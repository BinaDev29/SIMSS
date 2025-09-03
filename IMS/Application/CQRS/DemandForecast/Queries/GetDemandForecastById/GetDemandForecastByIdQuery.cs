using Application.DTOs.DemandForecast;
using MediatR;

namespace Application.CQRS.DemandForecast.Queries.GetDemandForecastById
{
    public class GetDemandForecastByIdQuery : IRequest<DemandForecastDto?>
    {
        public required int Id { get; set; }
    }
}