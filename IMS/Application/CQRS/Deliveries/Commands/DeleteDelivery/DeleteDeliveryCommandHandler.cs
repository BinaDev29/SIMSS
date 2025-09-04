using MediatR;
using Application.Contracts;
using Application.Responses;

namespace Application.CQRS.Delivery.Commands.DeleteDelivery
{
    public class DeleteDeliveryCommandHandler : IRequestHandler<DeleteDeliveryCommand, BaseCommandResponse>
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeleteDeliveryCommandHandler(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<BaseCommandResponse> Handle(DeleteDeliveryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var delivery = await _deliveryRepository.GetByIdAsync(request.Id, cancellationToken);

            if (delivery == null)
            {
                response.Success = false;
                response.Message = "Delivery not found for deletion.";
                return response;
            }

            await _deliveryRepository.DeleteAsync(delivery, cancellationToken);
            response.Success = true;
            response.Message = "Delivery deleted successfully.";
            return response;
        }
    }
}