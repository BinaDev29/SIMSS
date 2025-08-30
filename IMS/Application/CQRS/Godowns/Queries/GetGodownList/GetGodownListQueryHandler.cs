using MediatR;
using Application.Contracts;
using Application.DTOs.Godown;
using AutoMapper;
using System.Collections.Generic;

namespace Application.CQRS.Godowns.Queries.GetGodownList
{
    public class GetGodownListQueryHandler(IGodownRepository godownRepository, IMapper mapper)
        : IRequestHandler<GetGodownListQuery, List<GodownDto>>
    {
        public async Task<List<GodownDto>> Handle(GetGodownListQuery request, CancellationToken cancellationToken)
        {
            var godowns = await godownRepository.GetAllAsync(cancellationToken);
            return mapper.Map<List<GodownDto>>(godowns);
        }
    }
}