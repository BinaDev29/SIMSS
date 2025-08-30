using Application.Contracts;
using Application.DTOs.Supplier;
using Application.DTOs.Supplier.Validators;
using Application.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.CQRS.Suppliers.Commands.UpdateSupplier
{
    public class UpdateSupplierCommandHandler(
        ISupplierRepository supplierRepository,
        IMapper mapper,
        IValidator<UpdateSupplierDto> validator)
        : IRequestHandler<UpdateSupplierCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.SupplierDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Supplier update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken);
            if (supplier == null)
            {
                response.Success = false;
                response.Message = "Supplier not found.";
                return response;
            }

            mapper.Map(request.SupplierDto, supplier);
            await supplierRepository.UpdateAsync(supplier, cancellationToken);

            response.Success = true;
            response.Message = "Supplier updated successfully.";
            response.Id = supplier.Id;
            return response;
        }
    }
}