using Application.DTOs.Employee;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Employee.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommand : IRequest<BaseCommandResponse>
    {
        public required UpdateEmployeeDto EmployeeDto { get; set; }
    }
}