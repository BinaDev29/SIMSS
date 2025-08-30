using Application.Contracts;
using Application.DTOs.Employee;
using AutoMapper;
using MediatR;

namespace Application.CQRS.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<EmployeeDto>(employee);
        }
    }
}