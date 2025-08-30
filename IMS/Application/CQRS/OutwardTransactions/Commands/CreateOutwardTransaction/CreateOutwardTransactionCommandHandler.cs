using Application.Contracts;
using Application.CQRS.Transactions.Commands.CreateOutwardTransaction;
using Application.DTOs.Transaction.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.CQRS.OutwardTransactions.Commands.CreateOutwardTransaction
{
    public class CreateOutwardTransactionCommandHandler(IOutwardTransactionRepository outwardTransactionRepository, IItemRepository itemRepository, IMapper mapper)
        : IRequestHandler<CreateOutwardTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateOutwardTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateOutwardTransactionValidator();
            var validationResult = await validator.ValidateAsync(request.OutwardTransactionDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Outward Transaction creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var outwardTransaction = mapper.Map<OutwardTransaction>(request.OutwardTransactionDto);
            await outwardTransactionRepository.AddAsync(outwardTransaction, cancellationToken);

            // Update item stock
            var item = await itemRepository.GetByIdAsync(outwardTransaction.ItemId, cancellationToken);
            if (item != null)
            {
                item.StockQuantity -= outwardTransaction.QuantityDelivered;
                await itemRepository.UpdateAsync(item, cancellationToken);
            }

            response.Success = true;
            response.Message = "Outward Transaction created successfully.";
            response.Id = outwardTransaction.Id;
            return response;
        }
    }
}