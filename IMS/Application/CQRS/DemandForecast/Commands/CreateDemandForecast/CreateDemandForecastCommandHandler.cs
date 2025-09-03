using MediatR;
using Application.Contracts;
using Application.Responses;
using AutoMapper;

namespace Application.CQRS.DemandForecast.Commands.CreateDemandForecast
{
    public class CreateDemandForecastCommandHandler : IRequestHandler<CreateDemandForecastCommand, BaseCommandResponse>
    {
        private readonly IDemandForecastRepository _demandForecastRepository;
        private readonly IMapper _mapper;

        public CreateDemandForecastCommandHandler(IDemandForecastRepository demandForecastRepository, IMapper mapper)
        {
            _demandForecastRepository = demandForecastRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreateDemandForecastCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            var demandForecast = _mapper.Map<Domain.Models.DemandForecast>(request.DemandForecastDto);
            var addedForecast = await _demandForecastRepository.AddAsync(demandForecast, cancellationToken);

            response.Success = true;
            response.Message = "Demand forecast created successfully.";
            response.Id = addedForecast.Id;

            return response;
        }
    }
}