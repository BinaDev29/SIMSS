using MediatR;
using Application.Contracts;
using Application.DTOs.Transaction;
using Application.DTOs.Transaction.Validators;
using Application.Responses;
using Application.Services;
using AutoMapper;
using System.Transactions;

namespace Application.CQRS.Transactions.Commands.CreateReturnTransaction
{
    public class CreateReturnTransactionCommandHandler(
        IReturnTransactionRepository returnTransactionRepository,
        IGodownInventoryService godownInventoryService,
        IMapper mapper)
        : IRequestHandler<CreateReturnTransactionCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateReturnTransactionCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateReturnTransactionValidator();
            var validationResult = await validator.ValidateAsync(request.ReturnTransactionDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Return Transaction creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // 💡 ሁሉንም ኦፕሬሽኖች በአንድ ትራንስአክሽን ውስጥ ያጠቃልላል
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var returnTransaction = mapper.Map<Domain.Models.ReturnTransaction>(request.ReturnTransactionDto);
                var addedTransaction = await returnTransactionRepository.AddAsync(returnTransaction, cancellationToken);

                // 💡 የ GodownInventoryን መጠን ይጨምራል
                await godownInventoryService.UpdateInventoryQuantity(
                    addedTransaction.GodownId,
                    addedTransaction.ItemId,
                    addedTransaction.Quantity,
                    cancellationToken);

                scope.Complete();

                response.Success = true;
                response.Message = "Return Transaction created and inventory updated successfully.";
                response.Id = addedTransaction.Id;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Return Transaction creation failed.";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}