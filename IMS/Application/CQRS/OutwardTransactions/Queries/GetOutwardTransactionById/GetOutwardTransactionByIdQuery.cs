// GetOutwardTransactionByIdQuery.cs
using MediatR;
using Application.DTOs.Transaction;

namespace Application.CQRS.Transactions.Queries.GetOutwardTransactionById
{
    public class GetOutwardTransactionByIdQuery : IRequest<OutwardTransactionDto>
    {
        public required int Id { get; set; }
    }
}