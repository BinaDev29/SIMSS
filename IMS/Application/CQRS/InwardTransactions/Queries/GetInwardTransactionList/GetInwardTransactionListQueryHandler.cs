using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using Application.Responses;
using Application.DTOs.Common;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Transactions.Queries.GetInwardTransactionList
{
    public class GetInwardTransactionListQueryHandler(IInwardTransactionRepository inwardTransactionRepository, IMapper mapper)
        : IRequestHandler<GetInwardTransactionListQuery, PagedResponse<InwardTransactionDto>>
    {
        public async Task<PagedResponse<InwardTransactionDto>> Handle(GetInwardTransactionListQuery request, CancellationToken cancellationToken)
        {
            // 💡 በገጽ የተከፋፈለ እና የተጣራ የግብይቶች ዝርዝር ያመጣል
            var pagedResult = await inwardTransactionRepository.GetPagedInwardTransactionsAsync(
                request.Parameters.PageNumber,
                request.Parameters.PageSize,
                request.Parameters.SearchTerm,
                cancellationToken);

            var transactionDtos = mapper.Map<List<InwardTransactionDto>>(pagedResult.Items);

            return new PagedResponse<InwardTransactionDto>(
                transactionDtos,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pagedResult.PageSize);
        }
    }
}