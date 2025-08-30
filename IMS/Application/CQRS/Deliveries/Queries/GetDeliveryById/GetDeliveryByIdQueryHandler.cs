using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery;
using AutoMapper;

namespace Application.CQRS.Deliveries.Queries.GetDeliveryById
{
    public class GetDeliveryByIdQueryHandler(IDeliveryRepository deliveryRepository, IMapper mapper)
        : IRequestHandler<GetDeliveryByIdQuery, DeliveryDto>
    {
        public async Task<DeliveryDto> Handle(GetDeliveryByIdQuery request, CancellationToken cancellationToken)
        {
            var delivery = await deliveryRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<DeliveryDto>(delivery);
        }
    }
}