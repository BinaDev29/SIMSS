using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Transactions.Commands.DeleteOutwardTransaction
{
    public class DeleteOutwardTransactionCommandHandler(IOutwardTransactionRepository outwardTransactionRepository)
        : IRequestHandler<DeleteOutwardTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteOutwardTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var transactionToDelete = await outwardTransactionRepository.GetByIdAsync(request.Id, cancellationToken);

            if (transactionToDelete == null)
            {
                response.Success = false;
                response.Message = "Outward Transaction not found.";
                return response;
            }

            // 💡 Deleting transactions is not allowed to maintain historical data integrity.
            response.Success = false;
            response.Message = "Deleting outward transactions is not allowed to maintain historical data integrity.";
            return response;
        }
    }
}