using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
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

            await customerRepository.DeleteAsync(customer, cancellationToken);

            response.Success = true;
            response.Message = "Customer deleted successfully.";
            return response;
        }
    }
}