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
            await using var transaction = await invoiceRepository.BeginTransactionAsync(cancellationToken);
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
                        await itemRepository.Update(item, cancellationToken);
                    }
                }

                // Delete all old invoice details
                await invoiceDetailRepository.DeleteRangeAsync(oldDetails, cancellationToken);

                // Add new details and deduct new stock
                if (request.InvoiceDto.InvoiceDetails != null)
                {
                    foreach (var detailDto in request.InvoiceDto.InvoiceDetails)
                    {
                        var newItem = await itemRepository.GetByIdAsync(detailDto.ItemId.GetValueOrDefault(), cancellationToken);
                        if (newItem == null)
                        {
                            throw new Exception($"Item with ID {detailDto.ItemId} not found.");
                        }

                        if (newItem.StockQuantity < detailDto.Quantity.GetValueOrDefault())
                        {
                            throw new Exception($"Insufficient stock for item with ID {newItem.Id}. Available: {newItem.StockQuantity}, Requested: {detailDto.Quantity}");
                        }

                        newItem.StockQuantity -= detailDto.Quantity.GetValueOrDefault();
                        await itemRepository.Update(newItem, cancellationToken);

                        var newInvoiceDetail = mapper.Map<Domain.Models.InvoiceDetail>(detailDto);
                        newInvoiceDetail.InvoiceId = existingInvoice.Id;
                        await invoiceDetailRepository.AddAsync(newInvoiceDetail, cancellationToken);
                    }
                }

                // Map and update the main invoice data
                mapper.Map(request.InvoiceDto, existingInvoice);
                await invoiceRepository.Update(existingInvoice, cancellationToken);

                // Commit the transaction
                await transaction.CommitAsync(cancellationToken);

                response.Success = true;
                response.Message = "Invoice updated successfully.";
                response.Id = existingInvoice.Id;
            }
            catch (Exception ex)
            {
                // Rollback the transaction on failure
                await transaction.RollbackAsync(cancellationToken);
                response.Success = false;
                response.Message = "Invoice update failed.";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}