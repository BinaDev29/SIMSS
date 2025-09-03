using MediatR;
using Application.DTOs.Transaction;
using Application.Responses;

namespace Application.CQRS.InwardTransactions.Commands.UpdateInwardTransaction
{
    public class UpdateInwardTransactionCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateInwardTransactionDto InwardTransactionDto { get; set; }
    }
}