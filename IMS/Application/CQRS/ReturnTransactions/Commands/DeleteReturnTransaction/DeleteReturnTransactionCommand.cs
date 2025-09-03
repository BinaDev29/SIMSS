// DeleteReturnTransactionCommand.cs
using MediatR;
using Application.Responses;

namespace Application.CQRS.Transactions.Commands.DeleteReturnTransaction
{
    public class DeleteReturnTransactionCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}