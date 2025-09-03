using MediatR;
using Application.Contracts;
using Application.DTOs.Godown;
using AutoMapper;

namespace Application.CQRS.Godown.Queries.GetGodownById
{
    public class GetGodownByIdQueryHandler : IRequestHandler<GetGodownByIdQuery, GodownDto?>
    {
        private readonly IGodownRepository _godownRepository;
        private readonly IMapper _mapper;

        public GetGodownByIdQueryHandler(IGodownRepository godownRepository, IMapper mapper)
        {
            _godownRepository = godownRepository;
            _mapper = mapper;
        }

        public async Task<GodownDto?> Handle(GetGodownByIdQuery request, CancellationToken cancellationToken)
        {
            var godown = await _godownRepository.GetByIdAsync(request.Id, cancellationToken);
            return godown == null ? null : _mapper.Map<GodownDto>(godown);
        }
    }
}