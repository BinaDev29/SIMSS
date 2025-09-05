// Application/CQRS/OutwardTransactions/Commands/CreateOutwardTransaction/CreateOutwardTransactionCommandHandler.cs
using Application.Contracts;
using Application.CQRS.Transactions.Commands.CreateOutwardTransaction;
using Application.DTOs.Transaction;
using Application.DTOs.Transaction.Validators;
using Application.Responses;
using Application.Services;
using AutoMapper;
using MediatR;
using System;
using System.Transactions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.CQRS.OutwardTransactions.Commands.CreateOutwardTransaction
{
    public class CreateOutwardTransactionCommandHandler(
        IOutwardTransactionRepository outwardTransactionRepository,
        IGodownInventoryService godownInventoryService,
        IMapper mapper) : IRequestHandler<CreateOutwardTransactionCommand, BaseCommandResponse>
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

            // Check for sufficient stock using the dedicated service.
            // ??? GetValueOrDefault()? ?? ??? ???????
            var hasSufficientStock = await godownInventoryService.CheckSufficientStock(
                request.OutwardTransactionDto.ItemId,
                request.OutwardTransactionDto.GodownId,
                request.OutwardTransactionDto.QuantityDelivered,
                cancellationToken);

            if (!hasSufficientStock)
            {
                response.Success = false;
                response.Message = "Insufficient stock to complete the outward transaction.";
                return response;
            }

            // All operations are wrapped in a transaction for atomicity.
            using var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            }, TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var outwardTransaction = mapper.Map<Domain.Models.OutwardTransaction>(request.OutwardTransactionDto);
                var addedTransaction = await outwardTransactionRepository.AddAsync(outwardTransaction, cancellationToken);

                // Update inventory using the service.
                await godownInventoryService.UpdateInventoryQuantity(
                    addedTransaction.GodownId,
                    addedTransaction.ItemId,
                    -addedTransaction.QuantityDelivered,
                    cancellationToken);

                transaction.Complete();

                response.Success = true;
                response.Message = "Outward Transaction created and inventory updated successfully.";
                response.Id = addedTransaction.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Outward Transaction creation failed.";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}