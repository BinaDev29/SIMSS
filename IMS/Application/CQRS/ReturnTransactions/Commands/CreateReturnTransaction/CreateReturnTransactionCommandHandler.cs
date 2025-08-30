using Application.Contracts;
using Application.CQRS.Transactions.Commands.CreateReturnTransaction;
using Application.DTOs.Transaction.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.CQRS.ReturnTransactions.Commands.CreateReturnTransaction
{
    public class CreateReturnTransactionCommandHandler(IReturnTransactionRepository returnTransactionRepository, IItemRepository itemRepository, IMapper mapper)
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

            var returnTransaction = mapper.Map<ReturnTransaction>(request.ReturnTransactionDto);
            await returnTransactionRepository.AddAsync(returnTransaction, cancellationToken);

            // Update item stock by adding the returned quantity
            var item = await itemRepository.GetByIdAsync(returnTransaction.ItemId, cancellationToken);
            if (item != null)
            {
                item.StockQuantity += returnTransaction.Quantity;
                await itemRepository.UpdateAsync(item, cancellationToken);
            }

            response.Success = true;
            response.Message = "Return Transaction created successfully.";
            response.Id = returnTransaction.Id;
            return response;
        }
    }
}