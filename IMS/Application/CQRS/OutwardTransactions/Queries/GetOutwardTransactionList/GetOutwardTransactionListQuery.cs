using MediatR;
using Application.DTOs.Transaction;
using Application.DTOs.Common;
using Application.Responses;

namespace Application.CQRS.Transactions.Queries.GetOutwardTransactionList
{
    public class GetOutwardTransactionListQuery : IRequest<PagedResponse<OutwardTransactionDto>>
    {
        public required OutwardTransactionQueryParameters Parameters { get; set; }
    }
}