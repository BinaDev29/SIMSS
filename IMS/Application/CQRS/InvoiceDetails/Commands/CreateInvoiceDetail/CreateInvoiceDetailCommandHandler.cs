using MediatR;
using Application.Contracts;
using Application.DTOs.Invoice.Validators;
using Application.Responses;
using AutoMapper;
using Application.Services;
using Domain.Models;

namespace Application.CQRS.InvoiceDetails.Commands.CreateInvoiceDetail
{
    public class CreateInvoiceDetailCommandHandler(IInvoiceDetailRepository invoiceDetailRepository, IGodownInventoryService godownInventoryService, IMapper mapper)
        : IRequestHandler<CreateInvoiceDetailCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateInvoiceDetailCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateInvoiceDetailValidator();
            var validationResult = await validator.ValidateAsync(request.InvoiceDetailDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Invoice detail creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // 💡 የመጋዘን ክምችት በቂ መሆኑን ያረጋግጣል
            var hasSufficientStock = await godownInventoryService.CheckSufficientStock(request.InvoiceDetailDto.ItemId, request.InvoiceDetailDto.GodownId, request.InvoiceDetailDto.Quantity, cancellationToken);
            if (!hasSufficientStock)
            {
                response.Success = false;
                response.Message = "Insufficient stock to create invoice detail.";
                return response;
            }

            var invoiceDetail = mapper.Map<Domain.Models.InvoiceDetail>(request.InvoiceDetailDto);
            var addedInvoiceDetail = await invoiceDetailRepository.AddAsync(invoiceDetail, cancellationToken);

            // 💡 ከመጋዘን ክምችት ይቀንሳል
            await godownInventoryService.UpdateInventoryQuantity(
                invoiceDetail.GodownId,
                invoiceDetail.ItemId,
                -invoiceDetail.Quantity,
                cancellationToken);

            response.Success = true;
            response.Message = "Invoice detail created successfully.";
            response.Id = addedInvoiceDetail.Id;

            return response;
        }
    }
}