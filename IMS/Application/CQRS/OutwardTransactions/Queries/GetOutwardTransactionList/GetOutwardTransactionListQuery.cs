// GetOutwardTransactionListQuery.cs
using MediatR;
using Application.DTOs.Transaction;
using System.Collections.Generic;

namespace Application.CQRS.Transactions.Queries.GetOutwardTransactionList
{
    public class GetOutwardTransactionListQuery : IRequest<List<OutwardTransactionDto>>
    {
    }
}