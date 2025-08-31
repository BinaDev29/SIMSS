using Application.Contracts;
using Application.DTOs.Item.Validators;
using Application.Responses;
using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

            // 💡 ተመሳሳይ ስም ወይም ኮድ ያለው እቃ መኖሩን ማረጋገጥ
            var itemExists = await itemRepository.GetItemByNameOrCodeAsync(request.ItemDto.ItemName, request.ItemDto.ItemCode, cancellationToken);
            if (itemExists != null)
            {
                response.Success = false;
                response.Message = "An item with this name or code already exists.";
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