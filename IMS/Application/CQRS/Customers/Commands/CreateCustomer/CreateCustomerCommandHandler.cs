using MediatR;
using Application.Contracts;
using Application.DTOs.Customer.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;

namespace Application.CQRS.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        : IRequestHandler<CreateCustomerCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateCustomerValidator();
            var validationResult = await validator.ValidateAsync(request.CustomerDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Customer creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var customer = mapper.Map<Domain.Models.Customer>(request.CustomerDto);
            var addedCustomer = await customerRepository.AddAsync(customer, cancellationToken);

            response.Success = true;
            response.Message = "Customer created successfully.";
            response.Id = addedCustomer.Id;

            return response;
        }
    }
}