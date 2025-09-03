// CreateOutwardTransactionCommand.cs
using MediatR;
using Application.DTOs.Transaction;
using Application.Responses;

namespace Application.CQRS.Transactions.Commands.CreateOutwardTransaction
{
    public class CreateOutwardTransactionCommand : IRequest<BaseCommandResponse>
    {
        public required CreateOutwardTransactionDto OutwardTransactionDto { get; set; }
    }
}