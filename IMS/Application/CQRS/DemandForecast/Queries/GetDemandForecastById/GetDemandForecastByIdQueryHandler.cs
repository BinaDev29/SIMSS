using MediatR;
using Application.Contracts;
using Application.DTOs.DemandForecast;
using AutoMapper;

namespace Application.CQRS.DemandForecast.Queries.GetDemandForecastById
{
    public class GetDemandForecastByIdQueryHandler : IRequestHandler<GetDemandForecastByIdQuery, DemandForecastDto?>
    {
        private readonly IDemandForecastRepository _demandForecastRepository;
        private readonly IMapper _mapper;

        public GetDemandForecastByIdQueryHandler(IDemandForecastRepository demandForecastRepository, IMapper mapper)
        {
            _demandForecastRepository = demandForecastRepository;
            _mapper = mapper;
        }

        public async Task<DemandForecastDto?> Handle(GetDemandForecastByIdQuery request, CancellationToken cancellationToken)
        {
            var forecast = await _demandForecastRepository.GetByIdAsync(request.Id, cancellationToken);
            return forecast == null ? null : _mapper.Map<DemandForecastDto>(forecast);
        }
    }
}