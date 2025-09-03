using MediatR;
using Application.Contracts;
using Application.DTOs.Godown;
using AutoMapper;

namespace Application.CQRS.Godown.Queries.GetGodownList
{
    public class GetGodownListQueryHandler : IRequestHandler<GetGodownListQuery, List<GodownDto>>
    {
        private readonly IGodownRepository _godownRepository;
        private readonly IMapper _mapper;

        public GetGodownListQueryHandler(IGodownRepository godownRepository, IMapper mapper)
        {
            _godownRepository = godownRepository;
            _mapper = mapper;
        }

        public async Task<List<GodownDto>> Handle(GetGodownListQuery request, CancellationToken cancellationToken)
        {
            var godowns = await _godownRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<GodownDto>>(godowns);
        }
    }
}