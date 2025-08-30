using Application.DTOs.Employee;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.Employees.Queries.GetEmployeeList
{
    public class GetEmployeeListQuery : IRequest<List<EmployeeDto>>
    {
    }
}