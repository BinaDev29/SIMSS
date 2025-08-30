using Application.DTOs.Customer;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<BaseCommandResponse>
    {
        public required UpdateCustomerDto CustomerDto { get; set; }
    }
}