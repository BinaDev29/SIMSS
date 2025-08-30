using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Deliveries.Commands.DeleteDelivery
{
    public class DeleteDeliveryCommandHandler(IDeliveryRepository deliveryRepository)
        : IRequestHandler<DeleteDeliveryCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(DeleteDeliveryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var delivery = await deliveryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (delivery == null)
            {
                response.Success = false;
                response.Message = "Delivery not found for deletion.";
                return response;
            }

            await deliveryRepository.DeleteAsync(delivery, cancellationToken);

            response.Success = true;
            response.Message = "Delivery deleted successfully.";
            return response;
        }
    }
}