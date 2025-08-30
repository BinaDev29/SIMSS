using Application.DTOs.Customer;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.Customers.Queries.GetCustomerList
{
    public class GetCustomerListQuery : IRequest<List<CustomerDto>>
    {
    }
}