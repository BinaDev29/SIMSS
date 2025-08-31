using Application.Contracts;
using Application.DTOs.Item;
using Application.DTOs.Item.Validators;
using Application.Responses;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.CQRS.Items.Commands.UpdateItem
{
    // The validator is now injected via the constructor for better performance.
    public class UpdateItemCommandHandler(IItemRepository itemRepository, IMapper mapper, IValidator<UpdateItemDto> validator)
        : IRequestHandler<UpdateItemCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            // Validate the DTO here.
            var validationResult = await validator.ValidateAsync(request.ItemDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Item update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // The ID is now correctly fetched from the command itself, not the DTO.
            var item = await itemRepository.GetByIdAsync(request.Id, cancellationToken);
            if (item == null)
            {
                response.Success = false;
                response.Message = "Item not found.";
                return response;
            }

            mapper.Map(request.ItemDto, item);
            await itemRepository.Update(item, cancellationToken);

            response.Success = true;
            response.Message = "Item updated successfully.";
            response.Id = item.Id; // Add the ID to the response for confirmation.
            return response;
        }
    }
}