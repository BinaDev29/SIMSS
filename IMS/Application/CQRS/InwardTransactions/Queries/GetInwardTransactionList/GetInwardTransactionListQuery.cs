using MediatR;
using Application.DTOs.Transaction;
using Application.DTOs.Common;
using Application.Responses;

namespace Application.CQRS.Transactions.Queries.GetInwardTransactionList
{
    public class GetInwardTransactionListQuery : IRequest<PagedResponse<InwardTransactionDto>>
    {
        public required InwardTransactionQueryParameters Parameters { get; set; }
    }
}