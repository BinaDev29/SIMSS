using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        : IRequestHandler<DeleteEmployeeCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var employee = await employeeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found for deletion.";
                return response;
            }

            await employeeRepository.DeleteAsync(employee, cancellationToken);

            response.Success = true;
            response.Message = "Employee deleted successfully.";
            return response;
        }
    }
}