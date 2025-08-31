using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using Application.Services;

namespace Application.CQRS.DeliveryDetails.Commands.CreateDeliveryDetail
{
    public class CreateDeliveryDetailCommandHandler(IDeliveryDetailRepository deliveryDetailRepository, IGodownInventoryService godownInventoryService, IMapper mapper)
        : IRequestHandler<CreateDeliveryDetailCommand, BaseCommandResponse>
    {
        private object? cancellationationToken;

        public object? GetCancellationationToken()
        {
            return cancellationationToken;
        }

        public async Task<BaseCommandResponse> Handle(CreateDeliveryDetailCommand request, object? cancellationationToken, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new CreateDeliveryDetailValidator();
            var validationResult = await validator.ValidateAsync(request.DeliveryDetailDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "DeliveryDetail creation failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
                return response;
            }

            // 💡 የመጋዘን ክምችት በቂ መሆኑን ማረጋገጥ አለብህ
            // ... (የክምችት ማጣሪያ ኮድ እዚህ ላይ ይገባል)

            var deliveryDetail = mapper.Map<Domain.Models.DeliveryDetail>(request.DeliveryDetailDto);
            var addedDeliveryDetail = await deliveryDetailRepository.AddAsync(deliveryDetail, cancellationationToken);

            // 💡 የመጋዘን ክምችትን ይቀንሳል
            await godownInventoryService.UpdateInventoryQuantity(
                addedDeliveryDetail.GodownId,
                addedDeliveryDetail.ItemId,
                -addedDeliveryDetail.Quantity);

            response.Success = true;
            response.Message = "DeliveryDetail created successfully.";
            response.Id = addedDeliveryDetail.Id;

            return response;
        }

        Task<BaseCommandResponse> IRequestHandler<CreateDeliveryDetailCommand, BaseCommandResponse>.Handle(CreateDeliveryDetailCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}