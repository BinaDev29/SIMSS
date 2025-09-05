// Application/CQRS/ReturnTransactions/Commands/UpdateReturnTransaction/UpdateReturnTransactionCommandHandler.cs
using Application.Contracts;
using Application.DTOs.Transaction;
using Application.DTOs.Transaction.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using System.Linq;

namespace Application.CQRS.ReturnTransactions.Commands.UpdateReturnTransaction
{
    public class UpdateReturnTransactionCommandHandler(
        IUnitOfWork unitOfWork, // የ IUnitOfWork dependency ጨምረናል
        IReturnTransactionRepository returnTransactionRepository,
        IItemRepository itemRepository,
        IMapper mapper,
        IValidator<UpdateReturnTransactionDto> validator)
        : IRequestHandler<UpdateReturnTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateReturnTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validationResult = await validator.ValidateAsync(request.ReturnTransactionDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Return Transaction update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var existingTransaction = await returnTransactionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existingTransaction == null)
            {
                response.Success = false;
                response.Message = "Return Transaction not found.";
                return response;
            }

            // ትክክለኛው የግብይት ጅምር: በ unitOfWork ላይ
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                // Revert the old stock change before updating the transaction
                var oldItem = await itemRepository.GetByIdAsync(existingTransaction.ItemId, cancellationToken);
                if (oldItem == null)
                {
                    throw new Exception("Original item not found. Cannot update stock.");
                }

                oldItem.StockQuantity -= existingTransaction.Quantity;

                // Map the new DTO data to the existing transaction object
                mapper.Map(request.ReturnTransactionDto, existingTransaction);

                // Find the new item (it might be the same as the old one)
                var newItem = await itemRepository.GetByIdAsync(existingTransaction.ItemId, cancellationToken);
                if (newItem == null)
                {
                    throw new Exception("New item not found. Cannot update stock.");
                }

                // Apply the new stock change
                newItem.StockQuantity += existingTransaction.Quantity;

                // ለውጦችን በሙሉ በአንድ ጊዜ ለማስቀመጥ unitOfWorkን ተጠቀም
                await unitOfWork.CommitAsync(cancellationToken);

                // የ transactionን ግብይት አጠናቅቅ
                await transaction.CommitAsync(cancellationToken);

                response.Success = true;
                response.Message = "Return Transaction updated successfully.";
                response.Id = existingTransaction.Id;
            }
            catch (Exception ex)
            {
                // ግብይቱን መልስ
                await transaction.RollbackAsync(cancellationToken);
                response.Success = false;
                response.Message = "Return Transaction update failed.";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}