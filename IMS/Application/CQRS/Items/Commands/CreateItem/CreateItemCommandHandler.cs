using Application.Contracts;
using Application.DTOs.Item.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Items.Commands.CreateItem
{
    public class CreateItemCommandHandler(IItemRepository itemRepository, IMapper mapper)
        : IRequestHandler<CreateItemCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateItemValidator();
            var validationResult = await validator.ValidateAsync(request.ItemDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Item creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            var item = mapper.Map<Domain.Models.Item>(request.ItemDto);
            var addedItem = await itemRepository.AddAsync(item, cancellationToken);

            response.Success = true;
            response.Message = "Item created successfully.";
            response.Id = addedItem.Id;

            return response;
        }
    }
}