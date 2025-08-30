using MediatR;
using Application.Contracts;
using Application.DTOs.Invoice.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using Application.DTOs.Invoice;

namespace Application.CQRS.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IItemRepository itemRepository, IInvoiceDetailRepository invoiceDetailRepository, IMapper mapper)
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

            // Check if Items in invoice details exist and if there is enough stock
            foreach (var detail in request.InvoiceDto.InvoiceDetails)
            {
                var item = await itemRepository.GetByIdAsync(detail.ItemId, cancellationToken);
                if (item == null)
                {
                    response.Success = false;
                    response.Message = $"Item with ID {detail.ItemId} not found.";
                    return response;
                }
                if (item.StockQuantity < detail.Quantity)
                {
                    response.Success = false;
                    response.Message = $"Not enough stock for item {item.ItemName}. Available: {item.StockQuantity}, Requested: {detail.Quantity}";
                    return response;
                }
            }

            var invoice = mapper.Map<Domain.Models.Invoice>(request.InvoiceDto);
            var addedInvoice = await invoiceRepository.AddAsync(invoice, cancellationToken);

            foreach (var detailDto in request.InvoiceDto.InvoiceDetails)
            {
                var invoiceDetail = mapper.Map<Domain.Models.InvoiceDetail>(detailDto);
                invoiceDetail.InvoiceId = addedInvoice.Id; // Associate with the newly created invoice
                await invoiceDetailRepository.AddAsync(invoiceDetail, cancellationToken);

                // Update item stock
                var item = await itemRepository.GetByIdAsync(detailDto.ItemId, cancellationToken);
                if (item != null)
                {
                    item.StockQuantity -= detailDto.Quantity;
                    await itemRepository.UpdateAsync(item, cancellationToken);
                }
            }

            response.Success = true;
            response.Message = "Invoice created successfully.";
            response.Id = addedInvoice.Id;

            return response;
        }
    }
}