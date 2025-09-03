using Application.DTOs.DemandForecast;
using Application.DTOs.Common;
using MediatR;

namespace Application.CQRS.DemandForecast.Queries.GetDemandForecasts
{
    public class GetDemandForecastsQuery : IRequest<PagedResult<DemandForecastDto>>
    {
        public InvoiceDetailQueryParameters? Parameters { get; set; }
    }
}