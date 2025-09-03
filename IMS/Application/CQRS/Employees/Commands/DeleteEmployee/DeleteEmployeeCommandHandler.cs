using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Employee.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, BaseCommandResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<BaseCommandResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var employee = await _employeeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found for deletion.";
                return response;
            }

            await _employeeRepository.Delete(employee, cancellationToken);
            response.Success = true;
            response.Message = "Employee deleted successfully.";
            return response;
        }
    }
}