using Application.DTOs.DemandForecast;
using Application.Responses;
using MediatR;

namespace Application.CQRS.DemandForecast.Commands.CreateDemandForecast
{
    public class CreateDemandForecastCommand : IRequest<BaseCommandResponse>
    {
        public required CreateDemandForecastDto DemandForecastDto { get; set; }
    }
}