using MediatR;
using Application.Contracts;
using Application.DTOs.Supplier;
using Application.DTOs.Supplier.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.Suppliers.Commands.CreateSupplier
{
    public class CreateSupplierCommandHandler(ISupplierRepository supplierRepository, IMapper mapper)
        : IRequestHandler<CreateSupplierCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateSupplierValidator();
            var validationResult = await validator.ValidateAsync(request.SupplierDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Supplier creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // 💡 ተመሳሳይ ስም ያለው አቅራቢ መኖሩን ማረጋገጥ
            var supplierExists = await supplierRepository.GetSupplierByNameAsync(request.SupplierDto.SupplierName, cancellationToken);
            if (supplierExists != null)
            {
                response.Success = false;
                response.Message = "A supplier with this name already exists.";
                return response;
            }

            var supplier = mapper.Map<Domain.Models.Supplier>(request.SupplierDto);
            var addedSupplier = await supplierRepository.AddAsync(supplier, cancellationToken);

            response.Success = true;
            response.Message = "Supplier created successfully.";
            response.Id = addedSupplier.Id;

            return response;
        }
    }
}