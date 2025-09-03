using Application.Responses;
using MediatR;

namespace Application.CQRS.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}