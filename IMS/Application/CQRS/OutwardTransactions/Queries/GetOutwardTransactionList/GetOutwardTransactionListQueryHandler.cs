using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using Application.Responses;
using Application.DTOs.Common;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Transactions.Queries.GetOutwardTransactionList
{
    public class GetOutwardTransactionListQueryHandler(IOutwardTransactionRepository outwardTransactionRepository, IMapper mapper)
        : IRequestHandler<GetOutwardTransactionListQuery, PagedResponse<OutwardTransactionDto>>
    {
        public async Task<PagedResponse<OutwardTransactionDto>> Handle(GetOutwardTransactionListQuery request, CancellationToken cancellationToken)
        {
            // 💡 Get paged and filtered list of transactions.
            var pagedResult = await outwardTransactionRepository.GetPagedOutwardTransactionsAsync(
                request.Parameters.PageNumber,
                request.Parameters.PageSize,
                request.Parameters.SearchTerm,
                cancellationToken);

            var transactionDtos = mapper.Map<List<OutwardTransactionDto>>(pagedResult.Items);

            return new PagedResponse<OutwardTransactionDto>(
                transactionDtos,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pagedResult.PageSize);
        }
    }
}