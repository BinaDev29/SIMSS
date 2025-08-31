// Application/CQRS/Customers/Queries/GetCustomerList/GetCustomerListQuery.cs
using MediatR;
using Application.DTOs.Customer;
using Application.DTOs.Common;

namespace Application.CQRS.Customers.Queries.GetCustomerList
{
    public class GetCustomerListQuery : IRequest<List<CustomerDto>>
    {
        public CustomerQueryParameters? Parameters { get; set; }
    }
}