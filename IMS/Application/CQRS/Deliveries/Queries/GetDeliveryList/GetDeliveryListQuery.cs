using Application.DTOs.Delivery;
using MediatR;

namespace Application.CQRS.Delivery.Queries.GetDeliveryList
{
    public class GetDeliveryListQuery : IRequest<List<DeliveryDto>>
    {
        // Add filtering, paging, sorting parameters if needed
    }
}