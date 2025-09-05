// Application/CQRS/Customers/Commands/DeleteCustomer/DeleteCustomerCommandHandler.cs
using MediatR;
using Application.Contracts;
using Application.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Application.CQRS.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, BaseCommandResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
                if (customer == null)
                {
                    response.Success = false;
                    response.Message = "Customer not found.";
                    return response;
                }

                // Check if customer has transactions
                var hasTransactions = await _customerRepository.HasTransactionsByCustomerIdAsync(request.Id, cancellationToken);
                if (hasTransactions)
                {
                    response.Success = false;
                    response.Message = "Cannot delete customer with existing transactions.";
                    return response;
                }

                await _customerRepository.DeleteAsync(customer, cancellationToken);
                // እዚህ ላይ SaveAsyncን ወደ CommitAsync ቀይረናል
                await _unitOfWork.CommitAsync(cancellationToken);

                response.Success = true;
                response.Message = "Customer deleted successfully.";
                response.Id = request.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "An error occurred while deleting the customer.";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}