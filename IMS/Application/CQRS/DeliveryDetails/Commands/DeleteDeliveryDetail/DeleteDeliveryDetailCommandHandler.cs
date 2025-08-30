using Application.Contracts;
using Application.Responses;
using MediatR;

namespace Application.CQRS.DeliveryDetails.Commands.DeleteDeliveryDetail
{
    public class DeleteDeliveryDetailCommandHandler : IRequestHandler<DeleteDeliveryDetailCommand, BaseCommandResponse>
    {
        private readonly IDeliveryDetailRepository _repository;

        public DeleteDeliveryDetailCommandHandler(IDeliveryDetailRepository repository)
        {
            _repository = repository;
        }

        public async Task<BaseCommandResponse> Handle(DeleteDeliveryDetailCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            var deliveryDetail = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (deliveryDetail == null)
            {
                response.Success = false;
                response.Message = "DeliveryDetail not found";
                return response;
            }

            await _repository.DeleteAsync(deliveryDetail, cancellationToken);

            response.Success = true;
            response.Message = "DeliveryDetail deleted successfully";
            response.Id = request.Id;

            return response;
        }
    }
}
