using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using AutoMapper;
using System.Collections.Generic;

namespace Application.CQRS.Transactions.Queries.GetInwardTransactionList
{
    public class GetInwardTransactionListQueryHandler(IInwardTransactionRepository inwardTransactionRepository, IMapper mapper)
        : IRequestHandler<GetInwardTransactionListQuery, List<InwardTransactionDto>>
    {
        public async Task<List<InwardTransactionDto>> Handle(GetInwardTransactionListQuery request, CancellationToken cancellationToken)
        {
            var transactions = await inwardTransactionRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<InwardTransactionDto>>(transactions);
        }
    }
}