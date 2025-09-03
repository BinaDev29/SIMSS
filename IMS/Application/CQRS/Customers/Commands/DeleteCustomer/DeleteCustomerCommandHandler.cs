using MediatR;
using Application.Contracts;
using Application.Responses;
using System;

namespace Application.CQRS.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler(ICustomerRepository customerRepository, IOutwardTransactionRepository outwardTransactionRepository)
        : IRequestHandler<DeleteCustomerCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var customer = await customerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (customer == null)
            {
                response.Success = false;
                response.Message = "Customer not found for deletion.";
                return response;
            }

            // 💡 ከደንበኛው ጋር የተያያዘ ግብይት መኖሩን ማረጋገጥ
            var hasTransactions = await outwardTransactionRepository.HasTransactionsByCustomerId(request.Id);
            if (hasTransactions)
            {
                response.Success = false;
                response.Message = "Cannot delete customer because there are existing transactions linked to them.";
                return response;
            }

            await customerRepository.Delete(customer, cancellationToken);
            response.Success = true;
            response.Message = "Customer deleted successfully.";
            return response;
        }
    }
}