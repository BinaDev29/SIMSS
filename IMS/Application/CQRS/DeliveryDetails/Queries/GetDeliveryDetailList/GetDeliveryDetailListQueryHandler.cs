using Application.Contracts;
using Application.DTOs.Delivery;
using AutoMapper;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.DeliveryDetails.Queries.GetDeliveryDetailList
{
    public class GetDeliveryDetailListQueryHandler(IDeliveryDetailRepository deliveryDetailRepository, IMapper mapper)
        : IRequestHandler<GetDeliveryDetailListQuery, List<DeliveryDetailDto>>
    {
        public async Task<List<DeliveryDetailDto>> Handle(GetDeliveryDetailListQuery request, CancellationToken cancellationToken)
        {
            var deliveryDetails = await deliveryDetailRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<DeliveryDetailDto>>(deliveryDetails);
        }
    }
}