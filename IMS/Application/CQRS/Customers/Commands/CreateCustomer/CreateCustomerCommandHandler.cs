// Application/CQRS/Customers/Commands/CreateCustomer/CreateCustomerCommandHandler.cs
using MediatR;
using Application.Contracts;
using Application.DTOs.Customer.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Application.CQRS.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, BaseCommandResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
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

            // Check if customer with email already exists
            var existingCustomer = await _customerRepository.GetCustomerByEmailAsync(request.CustomerDto.Email, cancellationToken);
            if (existingCustomer != null)
            {
                response.Success = false;
                response.Message = "A customer with this email address already exists.";
                return response;
            }

            try
            {
                var customer = _mapper.Map<Customer>(request.CustomerDto);
                var addedCustomer = await _customerRepository.AddAsync(customer, cancellationToken);
                // እዚህ ላይ SaveAsyncን ወደ CommitAsync ቀይረናል
                await _unitOfWork.CommitAsync(cancellationToken);

                response.Success = true;
                response.Message = "Customer created successfully.";
                response.Id = addedCustomer.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while creating the customer.";
                response.Errors.Add(ex.Message);
            }

            return response;
        }
    }
}