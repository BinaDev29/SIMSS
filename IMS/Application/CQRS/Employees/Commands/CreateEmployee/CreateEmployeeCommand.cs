using Application.DTOs.Employee;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<BaseCommandResponse>
    {
        public required CreateEmployeeDto EmployeeDto { get; set; }
    }
}