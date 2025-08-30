// GetInwardTransactionByIdQuery.cs
using MediatR;
using Application.DTOs.Transaction;

namespace Application.CQRS.Transactions.Queries.GetInwardTransactionById
{
    public class GetInwardTransactionByIdQuery : IRequest<InwardTransactionDto>
    {
        public required int Id { get; set; }
    }
}