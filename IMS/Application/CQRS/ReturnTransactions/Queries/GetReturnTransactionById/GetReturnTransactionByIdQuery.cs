// GetReturnTransactionByIdQuery.cs
using MediatR;
using Application.DTOs.Transaction;

namespace Application.CQRS.Transactions.Queries.GetReturnTransactionById
{
    public class GetReturnTransactionByIdQuery : IRequest<ReturnTransactionDto>
    {
        public required int Id { get; set; }
    }
}