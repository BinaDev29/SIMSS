using MediatR;
using Application.DTOs.Transaction;
using Application.Responses;

namespace Application.CQRS.OutwardTransactions.Commands.UpdateOutwardTransaction
{
    public class UpdateOutwardTransactionCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateOutwardTransactionDto OutwardTransactionDto { get; set; }
    }
}