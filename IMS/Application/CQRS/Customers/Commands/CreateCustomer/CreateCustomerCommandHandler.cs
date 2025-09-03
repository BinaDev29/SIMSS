using MediatR;
using Application.Contracts;
using Application.DTOs.Customer.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, BaseCommandResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateCustomerValidator();
            var validationResult = await validator.ValidateAsync(request.CustomerDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Customer creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var customerExists = await _customerRepository.GetCustomerByEmailAsync(request.CustomerDto.Email, cancellationToken);
            if (customerExists != null)
            {
                response.Success = false;
                response.Message = "A customer with this email address already exists.";
                return response;
            }

            var customer = _mapper.Map<Domain.Models.Customer>(request.CustomerDto);
            var addedCustomer = await _customerRepository.AddAsync(customer, cancellationToken);

            response.Success = true;
            response.Message = "Customer created successfully.";
            response.Id = addedCustomer.Id;

            return response;
        }
    }
}