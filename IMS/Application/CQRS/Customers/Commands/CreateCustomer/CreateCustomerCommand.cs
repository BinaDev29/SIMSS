using Application.DTOs.Customer;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<BaseCommandResponse>
    {
        public required CreateCustomerDto CustomerDto { get; set; }
    }
}