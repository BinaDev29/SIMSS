// CreateInwardTransactionCommand.cs
using MediatR;
using Application.DTOs.Transaction;
using Application.Responses;

namespace Application.CQRS.Transactions.Commands.CreateInwardTransaction
{
    public class CreateInwardTransactionCommand : IRequest<BaseCommandResponse>
    {
        public required CreateInwardTransactionDto InwardTransactionDto { get; set; }
    }
}