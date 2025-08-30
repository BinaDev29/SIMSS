using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using AutoMapper;

namespace Application.CQRS.Transactions.Queries.GetReturnTransactionById
{
    public class GetReturnTransactionByIdQueryHandler(IReturnTransactionRepository returnTransactionRepository, IMapper mapper)
        : IRequestHandler<GetReturnTransactionByIdQuery, ReturnTransactionDto>
    {
        public async Task<ReturnTransactionDto> Handle(GetReturnTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await returnTransactionRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<ReturnTransactionDto>(transaction);
        }
    }
}