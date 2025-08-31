using MediatR;
using Application.Contracts;
using Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Employees.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository, IOutwardTransactionRepository outwardTransactionRepository)
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

            // 💡 ሰራተኛው ከውጪ ግብይቶች ጋር የተገናኘ መሆኑን ማረጋገጥ
            var hasTransactions = await outwardTransactionRepository.HasTransactionsByEmployeeIdAsync(request.Id, cancellationToken);
            if (hasTransactions)
            {
                response.Success = false;
                response.Message = "Cannot delete employee because there are existing transactions linked to them.";
                return response;
            }

            await employeeRepository.Delete(employee, cancellationToken);
            response.Success = true;
            response.Message = "Employee deleted successfully.";
            return response;
        }
    }
}