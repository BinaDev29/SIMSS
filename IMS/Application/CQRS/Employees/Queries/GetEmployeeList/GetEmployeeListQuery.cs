using Application.DTOs.Employee;
using MediatR;

namespace Application.CQRS.Employee.Queries.GetEmployeeList
{
    public class GetEmployeeListQuery : IRequest<List<EmployeeDto>>
    {
        // Add filtering, paging, sorting parameters if needed
    }
}