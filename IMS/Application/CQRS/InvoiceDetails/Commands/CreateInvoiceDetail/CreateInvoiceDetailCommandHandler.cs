using MediatR;
using Application.Contracts;
using Application.DTOs.Invoice.Validators;
using Application.DTOs.Invoice;
using Application.Responses;
using AutoMapper;
using Domain.Models;

namespace Application.CQRS.InvoiceDetails.Commands.CreateInvoiceDetail
{
    public class CreateInvoiceDetailCommandHandler(IInvoiceDetailRepository invoiceDetailRepository, IMapper mapper)
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

            var invoiceDetail = mapper.Map<Domain.Models.InvoiceDetail>(request.InvoiceDetailDto);
            var addedInvoiceDetail = await invoiceDetailRepository.AddAsync(invoiceDetail, cancellationToken);

            response.Success = true;
            response.Message = "Invoice detail created successfully.";
            response.Id = addedInvoiceDetail.Id;

            return response;
        }
    }
}