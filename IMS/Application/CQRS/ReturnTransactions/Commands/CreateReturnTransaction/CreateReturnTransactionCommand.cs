// CreateReturnTransactionCommand.cs
using MediatR;
using Application.DTOs.Transaction;
using Application.Responses;

namespace Application.CQRS.Transactions.Commands.CreateReturnTransaction
{
    public class CreateReturnTransactionCommand : IRequest<BaseCommandResponse>
    {
        public required CreateReturnTransactionDto ReturnTransactionDto { get; set; }
    }
}