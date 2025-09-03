// Application/CQRS/InventoryAlerts/Commands/CreateInventoryAlert/CreateInventoryAlertCommand.cs
using Application.DTOs.InventoryAlert;
using MediatR;

namespace Application.CQRS.InventoryAlerts.Commands.CreateInventoryAlert
{
    public class CreateInventoryAlertCommand : IRequest<int>
    {
        public CreateInventoryAlertDto InventoryAlert { get; set; } = new();
    }
}