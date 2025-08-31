using MediatR;
using Application.Contracts;
using Application.DTOs.Customer;
using AutoMapper;

namespace Application.CQRS.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper) : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            return mapper.Map<CustomerDto>(await customerRepository.GetCustomerWithDetailsAsync(request.Id, cancellationToken));
        }
    }
}