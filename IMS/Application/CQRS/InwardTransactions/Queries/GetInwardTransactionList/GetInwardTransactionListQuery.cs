// GetInwardTransactionListQuery.cs
using MediatR;
using Application.DTOs.Transaction;
using System.Collections.Generic;

namespace Application.CQRS.Transactions.Queries.GetInwardTransactionList
{
    public class GetInwardTransactionListQuery : IRequest<List<InwardTransactionDto>>
    {
    }
}