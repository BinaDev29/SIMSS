// DeleteOutwardTransactionCommand.cs
using MediatR;
using Application.Responses;

namespace Application.CQRS.Transactions.Commands.DeleteOutwardTransaction
{
    public class DeleteOutwardTransactionCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}