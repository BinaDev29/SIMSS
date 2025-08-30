using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using AutoMapper;
using System.Collections.Generic;

namespace Application.CQRS.Transactions.Queries.GetReturnTransactionList
{
    public class GetReturnTransactionListQueryHandler(IReturnTransactionRepository returnTransactionRepository, IMapper mapper)
        : IRequestHandler<GetReturnTransactionListQuery, List<ReturnTransactionDto>>
    {
        public async Task<List<ReturnTransactionDto>> Handle(GetReturnTransactionListQuery request, CancellationToken cancellationToken)
        {
            var transactions = await returnTransactionRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<ReturnTransactionDto>>(transactions);
        }
    }
}