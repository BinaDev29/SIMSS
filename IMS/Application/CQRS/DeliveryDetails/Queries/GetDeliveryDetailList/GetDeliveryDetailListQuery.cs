using Application.DTOs.Delivery;
using MediatR;
using System.Collections.Generic;

namespace Application.CQRS.DeliveryDetails.Queries.GetDeliveryDetailList
{
    public class GetDeliveryDetailListQuery : IRequest<List<DeliveryDetailDto>>
    {
    }
}