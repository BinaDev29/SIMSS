// Application/CQRS/DeliveryDetails/Commands/CreateDeliveryDetail/CreateDeliveryDetailCommandHandler.cs
using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery.Validators;
using Application.Responses;
using AutoMapper;
using Domain.Models;
using Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.DeliveryDetails.Commands.CreateDeliveryDetail
{
    public class CreateDeliveryDetailCommandHandler(
        IDeliveryDetailRepository deliveryDetailRepository,
        IGodownInventoryService godownInventoryService,
        IMapper mapper) : IRequestHandler<CreateDeliveryDetailCommand, BaseCommandResponse>
    {
        public async Task<BaseCommandResponse> Handle(CreateDeliveryDetailCommand request, CancellationToken cancellationToken)
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

            // Check inventory availability
            // ... (inventory checking code should be added here)

            var deliveryDetail = mapper.Map<Domain.Models.DeliveryDetail>(request.DeliveryDetailDto);
            var addedDeliveryDetail = await deliveryDetailRepository.AddAsync(deliveryDetail, cancellationToken); // Ensure AddAsync returns the added entity

            // Update inventory quantity
            await godownInventoryService.UpdateInventoryQuantity(
                addedDeliveryDetail.GodownId,
                addedDeliveryDetail.ItemId,
                -addedDeliveryDetail.Quantity,
                cancellationToken);

            response.Success = true;
            response.Message = "DeliveryDetail created successfully.";
            response.Id = addedDeliveryDetail.Id;
            return response;
        }
    }
}