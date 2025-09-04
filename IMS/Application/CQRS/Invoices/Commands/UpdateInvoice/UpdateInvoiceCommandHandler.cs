using Application.Contracts;
using Application.DTOs.Invoice;
using Application.DTOs.Invoice.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.CQRS.Invoices.Commands.UpdateInvoice
{
    public class UpdateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IItemRepository itemRepository,
        IInvoiceDetailRepository invoiceDetailRepository,
        IMapper mapper,
        IValidator<UpdateInvoiceDto> validator)
        : IRequestHandler<UpdateInvoiceCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.InvoiceDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Invoice update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var existingInvoice = await invoiceRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existingInvoice == null)
            {
                response.Success = false;
                response.Message = "Invoice not found.";
                return response;
            }

            // Begin a single transaction for atomicity to ensure all operations succeed or fail together
            using var transaction = await invoiceRepository.BeginTransactionAsync();
            try
            {
                // Revert old stock changes by adding back the quantities of the old details
                var oldDetails = await invoiceDetailRepository.GetInvoiceDetailsByInvoiceIdAsync(existingInvoice.Id, cancellationToken);
                foreach (var oldDetail in oldDetails)
                {
                    var item = await itemRepository.GetByIdAsync(oldDetail.ItemId, cancellationToken);
                    if (item != null)
                    {
                        item.StockQuantity += oldDetail.Quantity;
                        await itemRepository.UpdateAsync(item, cancellationToken);
                    }
                }

                // Delete all old invoice details
                await invoiceDetailRepository.DeleteRangeAsync(oldDetails, cancellationToken);

                // Add new details and deduct new stock
                if (request.InvoiceDto.InvoiceDetails != null)
                {
                    foreach (var detailDto in request.InvoiceDto.InvoiceDetails)
                    {
                        var itemId = detailDto.ItemId.GetValueOrDefault();
                        var quantity = detailDto.Quantity.GetValueOrDefault();
                        
                        var newItem = await itemRepository.GetByIdAsync(itemId, cancellationToken);
                        if (newItem == null)
                        {
                            throw new Exception($"Item with ID {itemId} not found.");
                        }

                        if (newItem.StockQuantity < quantity)
                        {
                            throw new Exception($"Insufficient stock for item with ID {newItem.Id}. Available: {newItem.StockQuantity}, Requested: {quantity}");
                        }

                        newItem.StockQuantity -= quantity;
                        await itemRepository.Update(newItem, cancellationToken);

                        var newInvoiceDetail = mapper.Map<Domain.Models.InvoiceDetail>(detailDto);
                        newInvoiceDetail.InvoiceId = existingInvoice.Id;
                        await invoiceDetailRepository.AddAsync(newInvoiceDetail, cancellationToken);
                    }
                }

                // Map and update the main invoice data
                mapper.Map(request.InvoiceDto, existingInvoice);
                await invoiceRepository.UpdateAsync(existingInvoice, cancellationToken);

                // Commit the transaction
                transaction.Commit();

                response.Success = true;
                response.Message = "Invoice updated successfully.";
                response.Id = existingInvoice.Id;
            }
            catch (Exception ex)
            {
                // Rollback the transaction on failure
                transaction.Rollback();
                response.Success = false;
                response.Message = "Invoice update failed.";
                response.Errors = [ex.Message];
            }

            return response;
        }
    }
}