using Application.DTOs.Delivery;
using MediatR;

namespace Application.CQRS.Delivery.Queries.GetDeliveryById
{
    public class GetDeliveryByIdQuery : IRequest<DeliveryDto?>
    {
        public required int Id { get; set; }
    }
}