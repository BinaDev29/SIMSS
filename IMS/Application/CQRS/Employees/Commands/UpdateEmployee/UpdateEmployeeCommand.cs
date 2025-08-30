using Application.DTOs.Employee;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateEmployeeDto EmployeeDto { get; set; }
    }
}