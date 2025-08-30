using MediatR;
using Application.Contracts;
using Application.DTOs.Godown;
using AutoMapper;

namespace Application.CQRS.Godowns.Queries.GetGodownById
{
    public class GetGodownByIdQueryHandler(IGodownRepository godownRepository, IMapper mapper)
        : IRequestHandler<GetGodownByIdQuery, GodownDto>
    {
        public async Task<GodownDto> Handle(GetGodownByIdQuery request, CancellationToken cancellationToken)
        {
            var godown = await godownRepository.GetByIdAsync(request.Id, cancellationToken);
            return mapper.Map<GodownDto>(godown);
        }
    }
}