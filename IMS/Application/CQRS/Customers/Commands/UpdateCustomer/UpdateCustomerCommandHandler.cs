using MediatR;
using Application.Contracts;
using Application.DTOs.Customer.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, BaseCommandResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new UpdateCustomerValidator();
            var validationResult = await validator.ValidateAsync(request.CustomerDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Customer update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            try
            {
                var existingCustomer = await _customerRepository.GetByIdAsync(request.CustomerDto.Id, cancellationToken);
                if (existingCustomer == null)
                {
                    response.Success = false;
                    response.Message = "Customer not found.";
                    return response;
                }

                // Check if email is being changed and if new email already exists
                if (existingCustomer.Email != request.CustomerDto.Email)
                {
                    var customerWithEmail = await _customerRepository.GetCustomerByEmailAsync(request.CustomerDto.Email, cancellationToken);
                    if (customerWithEmail != null)
                    {
                        response.Success = false;
                        response.Message = "A customer with this email address already exists.";
                        return response;
                    }
                }

                _mapper.Map(request.CustomerDto, existingCustomer);
                await _customerRepository.UpdateAsync(existingCustomer, cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                response.Success = true;
                response.Message = "Customer updated successfully.";
                response.Id = existingCustomer.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while updating the customer.";
                response.Errors.Add(ex.Message);
            }

            return response;
        }
    }
}