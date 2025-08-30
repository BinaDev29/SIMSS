using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using AutoMapper;
using System.Collections.Generic;

namespace Application.CQRS.Transactions.Queries.GetOutwardTransactionList
{
    public class GetOutwardTransactionListQueryHandler(IOutwardTransactionRepository outwardTransactionRepository, IMapper mapper)
        : IRequestHandler<GetOutwardTransactionListQuery, List<OutwardTransactionDto>>
    {
        public async Task<List<OutwardTransactionDto>> Handle(GetOutwardTransactionListQuery request, CancellationToken cancellationToken)
        {
            var transactions = await outwardTransactionRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<OutwardTransactionDto>>(transactions);
        }
    }
}