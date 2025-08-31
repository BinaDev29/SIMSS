using MediatR;
using Application.Contracts;
using Application.Responses;
using System;

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

            // 💡 የመላኪያ መረጃን መሰረዝ የለብንም። የዳታ ታሪክን ለመጠበቅ ሁኔታውን (Status) መለወጥ እንችላለን።
            response.Success = false;
            response.Message = "Deleting delivery records is not allowed to maintain data integrity. Consider updating the delivery status instead.";
            return response;
        }
    }
}