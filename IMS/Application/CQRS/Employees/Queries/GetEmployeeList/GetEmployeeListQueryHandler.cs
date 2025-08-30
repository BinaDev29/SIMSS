using Application.Contracts;
using Application.DTOs.Employee;
using AutoMapper;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.Employees.Queries.GetEmployeeList
{
    public class GetEmployeeListQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        : IRequestHandler<GetEmployeeListQuery, List<EmployeeDto>>
    {
        public async Task<List<EmployeeDto>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            var employees = await employeeRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<EmployeeDto>>(employees);
        }
    }
}