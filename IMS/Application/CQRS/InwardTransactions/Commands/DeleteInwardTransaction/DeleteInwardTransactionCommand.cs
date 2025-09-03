using MediatR;
using Application.Responses;

namespace Application.CQRS.Transactions.Commands.DeleteInwardTransaction
{
    public class DeleteInwardTransactionCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}