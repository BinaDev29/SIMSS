using Application.Contracts;
using Application.DTOs.Invoice;
using Application.DTOs.Invoice.Validators;
using Application.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.CQRS.InvoiceDetails.Commands.UpdateInvoiceDetail
{
    public class UpdateInvoiceDetailCommandHandler(IInvoiceDetailRepository invoiceDetailRepository, IMapper mapper, IValidator<UpdateInvoiceDetailDto> validator)
        : IRequestHandler<UpdateInvoiceDetailCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateInvoiceDetailCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.InvoiceDetailDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Invoice detail update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var invoiceDetail = await invoiceDetailRepository.GetByIdAsync(request.Id, cancellationToken);
            if (invoiceDetail == null)
            {
                response.Success = false;
                response.Message = "Invoice detail not found.";
                return response;
            }

            mapper.Map(request.InvoiceDetailDto, invoiceDetail);
            await invoiceDetailRepository.UpdateAsync(invoiceDetail, cancellationToken);

            response.Success = true;
            response.Message = "Invoice detail updated successfully.";
            response.Id = invoiceDetail.Id;
            return response;
        }
    }
}