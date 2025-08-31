using Application.Contracts;
using Application.Responses;
using MediatR;
using System;

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

            // 💡 የመላኪያ ዝርዝርን በቀጥታ መሰረዝ የለብህም።
            response.Success = false;
            response.Message = "Deleting delivery details is not allowed to maintain data integrity. You can update the item status or create a return transaction to correct mistakes.";
            return response;
        }
    }
}