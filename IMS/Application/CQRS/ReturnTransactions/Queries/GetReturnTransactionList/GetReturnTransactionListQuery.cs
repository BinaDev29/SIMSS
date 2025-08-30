// GetReturnTransactionListQuery.cs
using MediatR;
using Application.DTOs.Transaction;
using System.Collections.Generic;

namespace Application.CQRS.Transactions.Queries.GetReturnTransactionList
{
    public class GetReturnTransactionListQuery : IRequest<List<ReturnTransactionDto>>
    {
    }
}