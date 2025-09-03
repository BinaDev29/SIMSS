// CreateSupplierCommand.cs
using MediatR;
using Application.DTOs.Supplier;
using Application.Responses;

namespace Application.CQRS.Suppliers.Commands.CreateSupplier
{
    public class CreateSupplierCommand : IRequest<BaseCommandResponse>
    {
        public required CreateSupplierDto SupplierDto { get; set; }
    }
}