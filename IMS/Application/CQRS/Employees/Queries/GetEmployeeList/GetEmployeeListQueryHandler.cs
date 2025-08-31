using MediatR;
using Application.Contracts;
using Application.DTOs.Employee;
using AutoMapper;

namespace Application.CQRS.Employees.Queries.GetEmployeeList
{
    public class GetEmployeeListQueryHandler(IGenericRepository<Domain.Models.Employee> employeeRepository, IMapper mapper) : IRequestHandler<GetEmployeeListQuery, List<EmployeeDto>>
    {
        public async Task<List<EmployeeDto>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            var employees = await employeeRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<EmployeeDto>>(employees);
        }
    }
}