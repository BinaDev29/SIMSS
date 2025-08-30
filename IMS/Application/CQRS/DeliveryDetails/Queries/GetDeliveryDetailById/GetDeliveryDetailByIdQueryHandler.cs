using Application.Contracts;
using Application.DTOs.Delivery;
using AutoMapper;
using MediatR;

namespace Application.CQRS.DeliveryDetails.Queries.GetDeliveryDetailById
{
    public class GetDeliveryDetailByIdQueryHandler(IDeliveryDetailRepository deliveryDetailRepository, IMapper mapper)
        : IRequestHandler<GetDeliveryDetailByIdQuery, DeliveryDetailDto>
    {
        public async Task<DeliveryDetailDto> Handle(GetDeliveryDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var deliveryDetail = await deliveryDetailRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<DeliveryDetailDto>(deliveryDetail);
        }
    }
}