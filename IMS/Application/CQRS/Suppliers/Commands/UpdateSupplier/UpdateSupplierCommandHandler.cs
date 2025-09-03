using MediatR;
using Application.Contracts;
using Application.DTOs.Supplier;
using Application.DTOs.Supplier.Validators;
using Application.Responses;
using AutoMapper;
using FluentValidation;

namespace Application.CQRS.Suppliers.Commands.UpdateSupplier
{
    public class UpdateSupplierCommandHandler(ISupplierRepository supplierRepository, IMapper mapper, IValidator<UpdateSupplierDto> validator)
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

            var supplierToUpdate = await supplierRepository.GetByIdAsync(request.Id, cancellationToken);
            if (supplierToUpdate == null)
            {
                response.Success = false;
                response.Message = "Supplier not found for update.";
                return response;
            }

            // 💡 ከአሁኑ የተለየ ስም ከሌላ አቅራቢ ጋር ተመሳሳይ መሆኑን ማረጋገጥ
            var existingSupplier = await supplierRepository.GetSupplierByNameAsync(request.SupplierDto.SupplierName, cancellationToken);
            if (existingSupplier != null && existingSupplier.Id != request.Id)
            {
                response.Success = false;
                response.Message = "A supplier with this name already exists.";
                return response;
            }

            mapper.Map(request.SupplierDto, supplierToUpdate);
            await supplierRepository.Update(supplierToUpdate, cancellationToken);

            response.Success = true;
            response.Message = "Supplier updated successfully.";
            response.Id = supplierToUpdate.Id;

            return response;
        }
    }
}