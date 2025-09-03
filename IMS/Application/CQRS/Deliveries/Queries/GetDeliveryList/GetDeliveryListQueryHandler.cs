using MediatR;
using Application.Contracts;
using Application.DTOs.Delivery;
using AutoMapper;

namespace Application.CQRS.Delivery.Queries.GetDeliveryList
{
    public class GetDeliveryListQueryHandler : IRequestHandler<GetDeliveryListQuery, List<DeliveryDto>>
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IMapper _mapper;

        public GetDeliveryListQueryHandler(IDeliveryRepository deliveryRepository, IMapper mapper)
        {
            _deliveryRepository = deliveryRepository;
            _mapper = mapper;
        }

        public async Task<List<DeliveryDto>> Handle(GetDeliveryListQuery request, CancellationToken cancellationToken)
        {
            var deliveries = await _deliveryRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<DeliveryDto>>(deliveries);
        }
    }
}