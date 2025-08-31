using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery;
using AutoMapper;

namespace Application.CQRS.DeliveryDetails.Queries.GetDeliveryDetailById
{
    public class GetDeliveryDetailByIdQueryHandler(IDeliveryDetailRepository deliveryDetailRepository, IMapper mapper) : IRequestHandler<GetDeliveryDetailByIdQuery, DeliveryDetailDto>
    {
        public async Task<DeliveryDetailDto> Handle(GetDeliveryDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var deliveryDetail = await deliveryDetailRepository.GetDeliveryDetailWithDetailsAsync(request.Id, cancellationToken);
            return mapper.Map<DeliveryDetailDto>(deliveryDetail);
        }
    }
}