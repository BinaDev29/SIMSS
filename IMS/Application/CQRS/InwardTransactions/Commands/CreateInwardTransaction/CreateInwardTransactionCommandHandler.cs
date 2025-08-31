using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using Application.DTOs.Transaction.Validators;
using Application.Responses;
using Application.Services;
using AutoMapper;
using System.Transactions;

namespace Application.CQRS.Transactions.Commands.CreateInwardTransaction
{
    public class CreateInwardTransactionCommandHandler(
        IInwardTransactionRepository inwardTransactionRepository,
        IItemRepository itemRepository,
        IGodownRepository godownRepository,
        IGodownInventoryService godownInventoryService,
        IMapper mapper)
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

            // 💡 እቃው እና መጋዘኑ መኖራቸውን ያረጋግጣል
            var itemExists = await itemRepository.GetByIdAsync(request.InwardTransactionDto.ItemId, cancellationToken) != null;
            var godownExists = await godownRepository.GetByIdAsync(request.InwardTransactionDto.GodownId, cancellationToken) != null;
            if (!itemExists || !godownExists)
            {
                response.Success = false;
                response.Message = "Invalid Item ID or Godown ID.";
                return response;
            }

            // 💡 ሁሉንም ኦፕሬሽኖች በአንድ ትራንስአክሽን ውስጥ ያጠቃልላል
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var inwardTransaction = mapper.Map<Domain.Models.InwardTransaction>(request.InwardTransactionDto);
                var addedTransaction = await inwardTransactionRepository.AddAsync(inwardTransaction, cancellationToken);

                // 💡 የ GodownInventoryን መጠን ይጨምራል
                await godownInventoryService.UpdateInventoryQuantity(
                    addedTransaction.GodownId,
                    addedTransaction.ItemId,
                    addedTransaction.QuantityReceived,
                    cancellationToken);

                scope.Complete();

                response.Success = true;
                response.Message = "Inward Transaction created and inventory updated successfully.";
                response.Id = addedTransaction.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Inward Transaction creation failed.";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}