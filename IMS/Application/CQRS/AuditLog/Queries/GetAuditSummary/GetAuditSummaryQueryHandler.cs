using MediatR;
using Application.Contracts;
using Application.DTOs.Audit;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.AuditLog.Queries.GetAuditSummary
{
    public class GetAuditSummaryQueryHandler : IRequestHandler<GetAuditSummaryQuery, AuditSummaryDto>
    {
        private readonly IAuditLogRepository _auditLogRepository;

        public GetAuditSummaryQueryHandler(IAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<AuditSummaryDto> Handle(GetAuditSummaryQuery request, CancellationToken cancellationToken)
        {
            var query = _auditLogRepository.Query()
                .Where(a => a.Timestamp >= request.FromDate && a.Timestamp <= request.ToDate);

            var totalActions = await query.CountAsync(cancellationToken);
            var uniqueUsers = await query.Select(a => a.UserId).Distinct().CountAsync(cancellationToken);
            var uniqueEntities = await query.Select(a => a.EntityName).Distinct().CountAsync(cancellationToken);

            var actionCounts = await query
                .GroupBy(a => a.Action)
                .Select(g => new { Action = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Action, g => g.Count, cancellationToken);

            var entityCounts = await query
                .GroupBy(a => a.EntityName)
                .Select(g => new { Entity = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Entity, g => g.Count, cancellationToken);

            var userActivityCounts = await query
                .GroupBy(a => a.UserName)
                .Select(g => new { User = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.User, g => g.Count, cancellationToken);

            return new AuditSummaryDto
            {
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                TotalActions = totalActions,
                UniqueUsers = uniqueUsers,
                UniqueEntities = uniqueEntities,
                ActionCounts = actionCounts,
                EntityCounts = entityCounts,
                UserActivityCounts = userActivityCounts
            };
        }
    }
}