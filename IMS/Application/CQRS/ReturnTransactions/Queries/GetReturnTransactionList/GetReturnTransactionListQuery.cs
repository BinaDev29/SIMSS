using MediatR;
using Application.DTOs.Transaction;
using Application.DTOs.Common;
using Application.Responses;

namespace Application.CQRS.Transactions.Queries.GetReturnTransactionList
{
    public class GetReturnTransactionListQuery : IRequest<PagedResponse<ReturnTransactionDto>>
    {
        public required ReturnTransactionQueryParameters Parameters { get; set; }
    }
}