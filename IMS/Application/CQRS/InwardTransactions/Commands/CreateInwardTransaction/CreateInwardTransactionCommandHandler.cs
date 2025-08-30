using Application.Contracts;
using Application.CQRS.Transactions.Commands.CreateInwardTransaction;
using Application.DTOs.Transaction.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.CQRS.InwardTransactions.Commands.CreateInwardTransaction
{
    public class CreateInwardTransactionCommandHandler(IInwardTransactionRepository inwardTransactionRepository, IItemRepository itemRepository, IMapper mapper)
        : IRequestHandler<CreateInwardTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateInwardTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateInwardTransactionValidator();
            var validationResult = await validator.ValidateAsync(request.InwardTransactionDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Inward Transaction creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var inwardTransaction = mapper.Map<InwardTransaction>(request.InwardTransactionDto);
            await inwardTransactionRepository.AddAsync(inwardTransaction, cancellationToken);

            // Update item stock
            var item = await itemRepository.GetByIdAsync(inwardTransaction.ItemId, cancellationToken);
            if (item != null)
            {
                item.StockQuantity += inwardTransaction.QuantityReceived;
                await itemRepository.UpdateAsync(item, cancellationToken);
            }

            response.Success = true;
            response.Message = "Inward Transaction created successfully.";
            response.Id = inwardTransaction.Id;
            return response;
        }
    }
}