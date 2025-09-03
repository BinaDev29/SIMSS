using MediatR;
using Application.Contracts;
using Application.DTOs.GodownInventory;
using AutoMapper;

namespace Application.CQRS.GodownInventory.Queries.GetGodownInventoryList
{
    public class GetGodownInventoryListQueryHandler : IRequestHandler<GetGodownInventoryListQuery, List<GodownInventoryDto>>
    {
        private readonly IGodownInventoryRepository _godownInventoryRepository;
        private readonly IMapper _mapper;

        public GetGodownInventoryListQueryHandler(IGodownInventoryRepository godownInventoryRepository, IMapper mapper)
        {
            _godownInventoryRepository = godownInventoryRepository;
            _mapper = mapper;
        }

        public async Task<List<GodownInventoryDto>> Handle(GetGodownInventoryListQuery request, CancellationToken cancellationToken)
        {
            var inventories = await _godownInventoryRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<GodownInventoryDto>>(inventories);
        }
    }
}