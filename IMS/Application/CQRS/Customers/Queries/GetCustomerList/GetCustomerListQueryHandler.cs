using MediatR;
using Application.Contracts;
using Application.DTOs.Customer;
using Application.DTOs.Common;
using AutoMapper;

namespace Application.CQRS.Customers.Queries.GetCustomerList
{
    public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, PagedResult<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerListQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<CustomerDto>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {
            var pagedCustomers = await _customerRepository.GetPagedCustomersAsync(
                request.PageNumber, 
                request.PageSize, 
                request.SearchTerm, 
                cancellationToken);

            var customerDtos = _mapper.Map<IReadOnlyList<CustomerDto>>(pagedCustomers.Items);

            return new PagedResult<CustomerDto>(
                customerDtos,
                pagedCustomers.TotalCount,
                pagedCustomers.PageNumber,
                pagedCustomers.PageSize);
        }
    }
}