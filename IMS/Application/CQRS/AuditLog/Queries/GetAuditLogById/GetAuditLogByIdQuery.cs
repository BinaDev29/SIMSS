using Application.DTOs.AuditLog;
using MediatR;

namespace Application.CQRS.AuditLog.Queries.GetAuditLogById
{
    public class GetAuditLogByIdQuery : IRequest<AuditLogDto?>
    {
        public required int Id { get; set; }
    }
}