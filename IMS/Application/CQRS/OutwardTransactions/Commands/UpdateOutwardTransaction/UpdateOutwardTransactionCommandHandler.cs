using Application.Contracts;
using Application.DTOs.Transaction;
using Application.DTOs.Transaction.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.CQRS.OutwardTransactions.Commands.UpdateOutwardTransaction
{
    public class UpdateOutwardTransactionCommandHandler(IOutwardTransactionRepository outwardTransactionRepository, IItemRepository itemRepository, IMapper mapper, IValidator<UpdateOutwardTransactionDto> validator)
        : IRequestHandler<UpdateOutwardTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateOutwardTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.OutwardTransactionDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Outward Transaction update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // Get the OLD transaction and the OLD item for stock reversion
            var existingTransaction = await outwardTransactionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existingTransaction == null)
            {
                response.Success = false;
                response.Message = "Outward Transaction not found.";
                return response;
            }

            var oldItem = await itemRepository.GetByIdAsync(existingTransaction.ItemId, cancellationToken);
            if (oldItem == null)
            {
                response.Success = false;
                response.Message = "Original item not found. Cannot update stock.";
                return response;
            }

            // Revert old stock change
            oldItem.StockQuantity += existingTransaction.QuantityDelivered;

            // Map new DTO data to the existing transaction object
            mapper.Map(request.OutwardTransactionDto, existingTransaction);

            // Get the NEW item to apply the stock change. This might be a different item.
            var newItem = await itemRepository.GetByIdAsync(existingTransaction.ItemId, cancellationToken);
            if (newItem == null)
            {
                response.Success = false;
                response.Message = "New item not found. Cannot update stock.";
                return response;
            }

            // Check if there is enough stock for the new transaction.
            if (newItem.StockQuantity < existingTransaction.QuantityDelivered)
            {
                response.Success = false;
                response.Message = $"Insufficient stock for the new item. Available: {newItem.StockQuantity}, Requested: {existingTransaction.QuantityDelivered}";
                return response;
            }

            // Apply new stock change
            newItem.StockQuantity -= existingTransaction.QuantityDelivered;

            await outwardTransactionRepository.UpdateAsync(existingTransaction, cancellationToken);
            await itemRepository.UpdateAsync(oldItem, cancellationToken);

            // Only update newItem if it's different from oldItem. This prevents redundant database calls.
            if (oldItem.Id != newItem.Id)
            {
                await itemRepository.UpdateAsync(newItem, cancellationToken);
            }

            response.Success = true;
            response.Message = "Outward Transaction updated successfully.";
            response.Id = existingTransaction.Id;
            return response;
        }
    }
}