using MediatR;
using Application.DTOs.Delivery;
using System.Collections.Generic;

namespace Application.CQRS.Deliveries.Queries.GetDeliveryList
{
    public class GetDeliveryListQuery : IRequest<List<DeliveryDto>>
    {
    }
}