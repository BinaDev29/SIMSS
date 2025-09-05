//using MediatR;
//using Application.Contracts;
//using Application.DTOs.DemandForecast;
//using Application.DTOs.Common;
//using AutoMapper;
//using Microsoft.EntityFrameworkCore;

//namespace Application.CQRS.DemandForecast.Queries.GetDemandForecasts
//{
//    public class GetDemandForecastsQueryHandler : IRequestHandler<GetDemandForecastsQuery, PagedResult<DemandForecastDto>>
//    {
//        private readonly IDemandForecastRepository _demandForecastRepository;
//        private readonly IMapper _mapper;

//        public GetDemandForecastsQueryHandler(IDemandForecastRepository demandForecastRepository, IMapper mapper)
//        {
//            _demandForecastRepository = demandForecastRepository;
//            _mapper = mapper;
//        }

//        public async Task<PagedResult<DemandForecastDto>> Handle(GetDemandForecastsQuery request, CancellationToken cancellationToken)
//        {
//            var query = _demandForecastRepository.Query();

//            // Apply filtering based on request.Parameters if needed

//            var totalCount = await query.CountAsync(cancellationToken);

//            var pageNumber = request.Parameters?.PageNumber ?? 1;
//            var pageSize = request.Parameters?.PageSize ?? 10;

//            var items = await query
//                .Skip((pageNumber - 1) * pageSize)
//                .Take(pageSize)
//                .ToListAsync(cancellationToken);

//            var dtos = _mapper.Map<List<DemandForecastDto>>(items);

//            return new PagedResult<DemandForecastDto>(dtos, totalCount, pageNumber, pageSize);
//        }
//    }
//}