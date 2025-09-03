using Application.DTOs.Employee;
using MediatR;

namespace Application.CQRS.Employee.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeDto?>
    {
        public required int Id { get; set; }
    }
}