using MediatR;
using Application.Contracts;
using Application.DTOs.Customer.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;

namespace Application.CQRS.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        : IRequestHandler<UpdateCustomerCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new UpdateCustomerValidator();
            var validationResult = await validator.ValidateAsync(request.CustomerDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Customer update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var customer = await customerRepository.GetByIdAsync(request.CustomerDto.Id, cancellationToken);
            if (customer == null)
            {
                response.Success = false;
                response.Message = "Customer not found.";
                return response;
            }

            mapper.Map(request.CustomerDto, customer);
            await customerRepository.UpdateAsync(customer, cancellationToken);

            response.Success = true;
            response.Message = "Customer updated successfully.";
            return response;
        }
    }
}