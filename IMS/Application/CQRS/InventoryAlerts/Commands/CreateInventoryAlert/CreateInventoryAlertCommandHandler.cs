// Application/CQRS/InventoryAlerts/Commands/CreateInventoryAlert/CreateInventoryAlertCommandHandler.cs
using Application.Contracts;
using AutoMapper;
using Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CQRS.InventoryAlerts.Commands.CreateInventoryAlert
{
    public class CreateInventoryAlertCommandHandler : IRequestHandler<CreateInventoryAlertCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateInventoryAlertCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateInventoryAlertCommand request, CancellationToken cancellationToken)
        {
            var inventoryAlert = _mapper.Map<InventoryAlert>(request.InventoryAlert);

            // ???? 'cancellationToken' ??? ?? ??????
            await _unitOfWork.InventoryAlertRepository.AddAsync(inventoryAlert, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return inventoryAlert.Id;
        }
    }
}