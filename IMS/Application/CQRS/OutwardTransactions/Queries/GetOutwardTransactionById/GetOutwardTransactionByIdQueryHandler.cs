using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using AutoMapper;

namespace Application.CQRS.Transactions.Queries.GetOutwardTransactionById
{
    public class GetOutwardTransactionByIdQueryHandler(IOutwardTransactionRepository outwardTransactionRepository, IMapper mapper)
        : IRequestHandler<GetOutwardTransactionByIdQuery, OutwardTransactionDto>
    {
        public async Task<OutwardTransactionDto> Handle(GetOutwardTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await outwardTransactionRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<OutwardTransactionDto>(transaction);
        }
    }
}