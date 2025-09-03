using MediatR;
using Application.Contracts;
using Application.DTOs.AuditLog;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.CQRS.AuditLog.Queries.GetAuditLogs
{
    public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, IReadOnlyList<AuditLogDto>>
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IMapper _mapper;

        public GetAuditLogsQueryHandler(IAuditLogRepository auditLogRepository, IMapper mapper)
        {
            _auditLogRepository = auditLogRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<AuditLogDto>> Handle(GetAuditLogsQuery request, CancellationToken cancellationToken)
        {
            var query = _auditLogRepository.Query();

            if (!string.IsNullOrWhiteSpace(request.EntityName))
                query = query.Where(a => a.EntityName == request.EntityName);

            if (!string.IsNullOrWhiteSpace(request.UserId))
                query = query.Where(a => a.UserId == request.UserId);

            if (!string.IsNullOrWhiteSpace(request.Action))
                query = query.Where(a => a.Action == request.Action);

            if (request.FromDate.HasValue)
                query = query.Where(a => a.Timestamp >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(a => a.Timestamp <= request.ToDate.Value);

            // Sorting
            query = (request.SortBy?.ToLower()) switch
            {
                "timestamp" => request.SortDescending ? query.OrderByDescending(a => a.Timestamp) : query.OrderBy(a => a.Timestamp),
                "entityname" => request.SortDescending ? query.OrderByDescending(a => a.EntityName) : query.OrderBy(a => a.EntityName),
                _ => query.OrderByDescending(a => a.Timestamp)
            };

            // Paging
            var skip = (request.PageNumber - 1) * request.PageSize;
            var auditLogs = await query.Skip(skip).Take(request.PageSize).ToListAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<AuditLogDto>>(auditLogs);
        }
    }
}