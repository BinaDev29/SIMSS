using Application.Contracts;
using Application.DTOs.Customer;
using AutoMapper;
using MediatR;

namespace Application.CQRS.Customers.Queries.GetCustomerList
{
    public class GetCustomerListQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        : IRequestHandler<GetCustomerListQuery, List<CustomerDto>>
    {
        public async Task<List<CustomerDto>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {
            var customers = await customerRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<CustomerDto>>(customers);
        }
    }
}