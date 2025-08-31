// Application/CQRS/SmartReorders/Commands/CreateSmartReorder/CreateSmartReorderCommandHandler.cs
using Application.Contracts;
using AutoMapper;
using Domain.Models;
using MediatR;

namespace Application.CQRS.SmartReorders.Commands.CreateSmartReorder
{
    public class CreateSmartReorderCommandHandler : IRequestHandler<CreateSmartReorderCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSmartReorderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSmartReorderCommand request, CancellationToken cancellationToken)
        {
            var smartReorder = _mapper.Map<SmartReorder>(request.SmartReorder);
            smartReorder.Status = "PENDING";
            
            await _unitOfWork.SmartReorderRepository.AddAsync(smartReorder);
            await _unitOfWork.CommitAsync(cancellationToken);
            
            return smartReorder.Id;
        }
    }
}