using MediatR;
using Application.Contracts;
using Application.DTOs.Invoice;
using Application.DTOs.Invoice.Validators;
using Application.Responses;
using AutoMapper;
using Application.Services;
using Domain.Models;
using System.Transactions;

namespace Application.CQRS.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IGodownInventoryService godownInventoryService,
        IInvoiceDetailRepository invoiceDetailRepository,
        IMapper mapper)
        : IRequestHandler<CreateInvoiceCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateInvoiceValidator();
            var validationResult = await validator.ValidateAsync(request.InvoiceDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Invoice creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // Check if sufficient stock exists
            foreach (var detailDto in request.InvoiceDto.InvoiceDetails)
            {
                var hasSufficientStock = await godownInventoryService.CheckSufficientStock(
                    detailDto.ItemId,
                    detailDto.GodownId,
                    detailDto.Quantity,
                    cancellationToken);

                if (!hasSufficientStock)
                {
                    response.Success = false;
                    response.Message = $"Insufficient stock for item with ID {detailDto.ItemId} in godown {detailDto.GodownId}.";
                    return response;
                }
            }

            // Execute all operations in a transaction
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var invoice = mapper.Map<Domain.Models.Invoice>(request.InvoiceDto);
                var addedInvoice = await invoiceRepository.AddAsync(invoice, cancellationToken);

                foreach (var detailDto in request.InvoiceDto.InvoiceDetails)
                {
                    var invoiceDetail = mapper.Map<Domain.Models.InvoiceDetail>(detailDto);
                    invoiceDetail.InvoiceId = addedInvoice.Id;
                    await invoiceDetailRepository.AddAsync(invoiceDetail, cancellationToken);

                    // Update inventory using the service
                    await godownInventoryService.UpdateInventoryQuantity(
                        invoiceDetail.GodownId,
                        invoiceDetail.ItemId,
                        -invoiceDetail.Quantity,
                        cancellationToken);
                }

                scope.Complete();
                response.Success = true;
                response.Message = "Invoice created successfully.";
                response.Id = addedInvoice.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Invoice creation failed.";
                response.Errors = [ex.Message];
            }

            return response;
        }
    }
}