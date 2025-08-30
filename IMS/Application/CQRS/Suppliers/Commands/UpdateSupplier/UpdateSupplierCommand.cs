using Application.DTOs.Supplier;
using Application.Responses;
using MediatR;

namespace Application.CQRS.Suppliers.Commands.UpdateSupplier
{
    public class UpdateSupplierCommand : IRequest<BaseCommandResponse>
    {
        public int Id { get; set; }
        public required UpdateSupplierDto SupplierDto { get; set; }
    }
}