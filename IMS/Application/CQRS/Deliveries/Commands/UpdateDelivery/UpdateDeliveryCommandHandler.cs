using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery.Validators;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.Delivery.Commands.UpdateDelivery
{
    public class UpdateDeliveryCommandHandler : IRequestHandler<UpdateDeliveryCommand, BaseCommandResponse>
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IMapper _mapper;

        public UpdateDeliveryCommandHandler(IDeliveryRepository deliveryRepository, IMapper mapper)
        {
            _deliveryRepository = deliveryRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdateDeliveryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            var validator = new UpdateDeliveryValidator();
            var validationResult = await validator.ValidateAsync(request.DeliveryDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Delivery update failed due to validation errors.";
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            var delivery = await _deliveryRepository.GetByIdAsync(request.DeliveryDto.Id, cancellationToken);
            if (delivery == null)
            {
                response.Success = false;
                response.Message = "Delivery not found.";
                return response;
            }

            _mapper.Map(request.DeliveryDto, delivery);
            await _deliveryRepository.UpdateAsync(delivery, cancellationToken);

            response.Success = true;
            response.Message = "Delivery updated successfully.";
            return response;
        }
    }
}