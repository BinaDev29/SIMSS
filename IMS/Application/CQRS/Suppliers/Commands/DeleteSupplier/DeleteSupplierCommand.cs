// DeleteSupplierCommand.cs
using MediatR;
using Application.Responses;

namespace Application.CQRS.Suppliers.Commands.DeleteSupplier
{
    public class DeleteSupplierCommand : IRequest<BaseCommandResponse>
    {
        public required int Id { get; set; }
    }
}