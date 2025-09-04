// Application/CQRS/Customers/Queries/GetCustomerList/GetCustomerListQuery.cs
using MediatR;
using Application.DTOs.Customer;
using Application.DTOs.Common;

namespace Application.CQRS.Customers.Queries.GetCustomerList
{
    public class GetCustomerListQuery : IRequest<PagedResult<CustomerDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
    }
}