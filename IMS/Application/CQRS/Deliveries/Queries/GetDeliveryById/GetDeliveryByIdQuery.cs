using MediatR;
using Application.DTOs.Delivery;

namespace Application.CQRS.Deliveries.Queries.GetDeliveryById
{
    public class GetDeliveryByIdQuery : IRequest<DeliveryDto>
    {
        public required int Id { get; set; }
    }
}