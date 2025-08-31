using MediatR;
using Application.Contracts;
using Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Transactions.Commands.DeleteInwardTransaction
{
    public class DeleteInwardTransactionCommandHandler(IInwardTransactionRepository inwardTransactionRepository)
        : IRequestHandler<DeleteInwardTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteInwardTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var transactionToDelete = await inwardTransactionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (transactionToDelete == null)
            {
                response.Success = false;
                response.Message = "Inward Transaction not found.";
                return response;
            }

            // 💡 ግብይቶችን መሰረዝ አይፈቀድም
            response.Success = false;
            response.Message = "Deleting inward transactions is not allowed to maintain historical data integrity.";
            return response;
        }
    }
}