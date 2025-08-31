using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using Application.Responses;
using Application.DTOs.Common;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.Transactions.Queries.GetReturnTransactionList
{
    public class GetReturnTransactionListQueryHandler(IReturnTransactionRepository returnTransactionRepository, IMapper mapper)
        : IRequestHandler<GetReturnTransactionListQuery, PagedResponse<ReturnTransactionDto>>
    {
        public async Task<PagedResponse<ReturnTransactionDto>> Handle(GetReturnTransactionListQuery request, CancellationToken cancellationToken)
        {
            // 💡 በገጽ የተከፋፈለ እና የተጣራ የግብይቶች ዝርዝር ያመጣል
            var pagedResult = await returnTransactionRepository.GetPagedReturnTransactionsAsync(
                request.Parameters.PageNumber,
                request.Parameters.PageSize,
                request.Parameters.SearchTerm,
                cancellationToken);

            var transactionDtos = mapper.Map<List<ReturnTransactionDto>>(pagedResult.Items);

            return new PagedResponse<ReturnTransactionDto>(
                transactionDtos,
                pagedResult.TotalCount,
                pagedResult.PageNumber,
                pagedResult.PageSize);
        }
    }
}