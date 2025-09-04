using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery;
using AutoMapper;

namespace Application.CQRS.Delivery.Queries.GetDeliveryById
{
    public class GetDeliveryByIdQueryHandler : IRequestHandler<GetDeliveryByIdQuery, DeliveryDto?>
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IMapper _mapper;

        public GetDeliveryByIdQueryHandler(IDeliveryRepository deliveryRepository, IMapper mapper)
        {
            _deliveryRepository = deliveryRepository;
            _mapper = mapper;
        }

        public async Task<DeliveryDto?> Handle(GetDeliveryByIdQuery request, CancellationToken cancellationToken)
        {
            var delivery = await deliveryRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
            return delivery == null ? null : _mapper.Map<DeliveryDto>(delivery);
        }
    }
}