using Application.Responses;
using MediatR;

namespace Application.CQRS.Employee.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}