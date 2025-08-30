using Application.Contracts;
using Application.DTOs.Customer;
using AutoMapper;
using MediatR;

namespace Application.CQRS.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await customerRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<CustomerDto>(customer);
        }
    }
}