using Application.Contracts;
using Application.DTOs.Transaction;
using Application.DTOs.Transaction.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using System.Linq;

namespace Application.CQRS.InwardTransactions.Commands.UpdateInwardTransaction
{
    public class UpdateInwardTransactionCommandHandler(
        IInwardTransactionRepository inwardTransactionRepository,
        IItemRepository itemRepository,
        IMapper mapper,
        IValidator<UpdateInwardTransactionDto> validator)
        : IRequestHandler<UpdateInwardTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateInwardTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.InwardTransactionDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Inward Transaction update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // Get the OLD transaction and the OLD item for stock reversion
            var existingTransaction = await inwardTransactionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existingTransaction == null)
            {
                response.Success = false;
                response.Message = "Inward Transaction not found.";
                return response;
            }

            var oldItem = await itemRepository.GetByIdAsync(existingTransaction.ItemId, cancellationToken);
            if (oldItem == null)
            {
                response.Success = false;
                response.Message = "Original item not found. Cannot update stock.";
                return response;
            }

            // Revert old stock change by subtracting the old quantity
            oldItem.StockQuantity -= existingTransaction.QuantityReceived;

            // Map new DTO data to the existing transaction object to update it
            mapper.Map(request.InwardTransactionDto, existingTransaction);

            // Get the NEW item to apply the stock change. This might be a different item.
            var newItem = await itemRepository.GetByIdAsync(existingTransaction.ItemId, cancellationToken);
            if (newItem == null)
            {
                response.Success = false;
                response.Message = "New item not found. Cannot update stock.";
                return response;
            }

            // Apply new stock change by adding the new quantity
            newItem.StockQuantity += existingTransaction.QuantityReceived;

            await inwardTransactionRepository.Update(existingTransaction, cancellationToken);
            await itemRepository.Update(oldItem, cancellationToken);

            // Only update newItem if it's different from oldItem. This prevents redundant database calls.
            if (oldItem.Id != newItem.Id)
            {
                await itemRepository.Update(newItem, cancellationToken);
            }

            response.Success = true;
            response.Message = "Inward Transaction updated successfully.";
            response.Id = existingTransaction.Id;
            return response;
        }
    }
}