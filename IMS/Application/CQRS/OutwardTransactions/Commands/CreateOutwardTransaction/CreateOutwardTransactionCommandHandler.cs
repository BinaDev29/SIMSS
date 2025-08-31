using Application.Contracts;
using Application.CQRS.Transactions.Commands.CreateOutwardTransaction;
using Application.DTOs.Transaction;
using Application.DTOs.Transaction.Validators;
using Application.Responses;
using Application.Services;
using AutoMapper;
using MediatR;
using System.Transactions;

public class CreateOutwardTransactionCommandHandler(
    IOutwardTransactionRepository outwardTransactionRepository,
    IGodownInventoryService godownInventoryService,
    IMapper mapper)
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

        // 💡 Check for sufficient stock using the dedicated service.
        var hasSufficientStock = await godownInventoryService.CheckSufficientStock(
            request.OutwardTransactionDto.ItemId.GetValueOrDefault(),
            request.OutwardTransactionDto.GodownId.GetValueOrDefault(),
            request.OutwardTransactionDto.QuantityDelivered.GetValueOrDefault(),
            cancellationToken);

        if (!hasSufficientStock)
        {
            response.Success = false;
            response.Message = "Insufficient stock to complete the outward transaction.";
            return response;
        }

        // 💡 All operations are wrapped in a transaction for atomicity.
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            var outwardTransaction = mapper.Map<Domain.Models.OutwardTransaction>(request.OutwardTransactionDto);
            var addedTransaction = await outwardTransactionRepository.AddAsync(outwardTransaction, cancellationToken);

            // 💡 Update inventory using the service.
            await godownInventoryService.UpdateInventoryQuantity(
                addedTransaction.GodownId,
                addedTransaction.ItemId,
                -addedTransaction.QuantityDelivered,
                cancellationToken);

            scope.Complete();

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