using Application.DTOs.Delivery;
using MediatR;

namespace Application.CQRS.DeliveryDetails.Queries.GetDeliveryDetailById
{
    public class GetDeliveryDetailByIdQuery : IRequest<DeliveryDetailDto>
    {
        public required int Id { get; set; }
    }
}