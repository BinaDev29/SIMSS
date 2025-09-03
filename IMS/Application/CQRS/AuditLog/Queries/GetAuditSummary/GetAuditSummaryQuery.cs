using Application.DTOs.Audit;
using MediatR;

namespace Application.CQRS.AuditLog.Queries.GetAuditSummary
{
    public class GetAuditSummaryQuery : IRequest<AuditSummaryDto>
    {
        public required DateTime FromDate { get; set; }
        public required DateTime ToDate { get; set; }
    }
}