using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Items.Commands.DeleteItem
{
    public class DeleteItemCommandHandler(IItemRepository itemRepository)
        : IRequestHandler<DeleteItemCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var item = await itemRepository.GetByIdAsync(request.Id, cancellationToken);

            if (item == null)
            {
                response.Success = false;
                response.Message = "Item not found for deletion.";
                return response;
            }

            await itemRepository.DeleteAsync(item, cancellationToken);

            response.Success = true;
            response.Message = "Item deleted successfully.";
            return response;
        }
    }
}