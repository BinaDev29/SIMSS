using MediatR;
using Application.Responses;

namespace Application.CQRS.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}