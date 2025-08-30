using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery;
using AutoMapper;
using System.Collections.Generic;

namespace Application.CQRS.Deliveries.Queries.GetDeliveryList
{
    public class GetDeliveryListQueryHandler(IDeliveryRepository deliveryRepository, IMapper mapper)
        : IRequestHandler<GetDeliveryListQuery, List<DeliveryDto>>
    {
        public async Task<List<DeliveryDto>> Handle(GetDeliveryListQuery request, CancellationToken cancellationToken)
        {
            var deliveries = await deliveryRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<DeliveryDto>>(deliveries);
        }
    }
}