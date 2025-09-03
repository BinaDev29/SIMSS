using Application.DTOs.Customer;
using MediatR;

namespace Application.CQRS.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public required int Id { get; set; }
    }
}