using MediatR;
using Application.Contracts;
using Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Transactions.Commands.DeleteReturnTransaction
{
    public class DeleteReturnTransactionCommandHandler(IReturnTransactionRepository returnTransactionRepository)
        : IRequestHandler<DeleteReturnTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteReturnTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var transactionToDelete = await returnTransactionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (transactionToDelete == null)
            {
                response.Success = false;
                response.Message = "Return Transaction not found.";
                return response;
            }

            // 💡 ግብይቶችን መሰረዝ አይፈቀድም
            response.Success = false;
            response.Message = "Deleting return transactions is not allowed to maintain historical data integrity.";
            return response;
        }
    }
}