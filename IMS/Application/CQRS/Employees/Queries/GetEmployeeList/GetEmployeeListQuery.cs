using MediatR;
using Application.DTOs.Employee;
using Application.DTOs.Common;

namespace Application.CQRS.Employees.Queries.GetEmployeeList
{
    public class GetEmployeeListQuery : IRequest<List<EmployeeDto>>
    {
        public EmployeeQueryParameters? Parameters { get; set; }
    }
}