using MediatR;
using Application.Contracts;
using Application.DTOs.GodownInventory;
using AutoMapper;

namespace Application.CQRS.GodownInventory.Queries.GetGodownInventoryById
{
    public class GetGodownInventoryByIdQueryHandler : IRequestHandler<GetGodownInventoryByIdQuery, GodownInventoryDto?>
    {
        private readonly IGodownInventoryRepository _godownInventoryRepository;
        private readonly IMapper _mapper;

        public GetGodownInventoryByIdQueryHandler(IGodownInventoryRepository godownInventoryRepository, IMapper mapper)
        {
            _godownInventoryRepository = godownInventoryRepository;
            _mapper = mapper;
        }

        public async Task<GodownInventoryDto?> Handle(GetGodownInventoryByIdQuery request, CancellationToken cancellationToken)
        {
            var inventory = await _godownInventoryRepository.GetByIdAsync(request.Id, cancellationToken);
            return inventory == null ? null : _mapper.Map<GodownInventoryDto>(inventory);
        }
    }
}