using MediatR;
using Application.DTOs.Transaction;
using Application.Responses;

namespace Application.CQRS.ReturnTransactions.Commands.UpdateReturnTransaction
{
    public class UpdateReturnTransactionCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateReturnTransactionDto ReturnTransactionDto { get; set; }
    }
}