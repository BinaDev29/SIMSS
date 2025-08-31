// Application/CQRS/SmartReorders/Commands/CreateSmartReorder/CreateSmartReorderCommand.cs
using Application.DTOs.SmartReorder;
using MediatR;

namespace Application.CQRS.SmartReorders.Commands.CreateSmartReorder
{
    public class CreateSmartReorderCommand : IRequest<int>
    {
        public CreateSmartReorderDto SmartReorder { get; set; } = new();
    }
}