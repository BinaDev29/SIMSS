using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using AutoMapper;

namespace Application.CQRS.Transactions.Queries.GetInwardTransactionById
{
    public class GetInwardTransactionByIdQueryHandler(IInwardTransactionRepository inwardTransactionRepository, IMapper mapper)
        : IRequestHandler<GetInwardTransactionByIdQuery, InwardTransactionDto>
    {
        public async Task<InwardTransactionDto> Handle(GetInwardTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await inwardTransactionRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<InwardTransactionDto>(transaction);
        }
    }
}