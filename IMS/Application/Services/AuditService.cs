// Application/Services/AuditService.cs
using Application.Contracts;
using Application.DTOs.Audit;
using Domain.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditLogRepository _auditRepository;
        private readonly IMapper _mapper;

        public AuditService(IAuditLogRepository auditRepository, IMapper mapper)
        {
            _auditRepository = auditRepository;
            _mapper = mapper;
        }

        public async Task LogAsync(string entityName, string entityId, string action, object? oldValues, object? newValues, string userId, string userName)
        {
            var auditLog = new AuditLog
            {
                EntityName = entityName,
                EntityId = entityId,
                Action = action,
                UserId = userId,
                UserName = userName,
                Details = $"Old: {System.Text.Json.JsonSerializer.Serialize(oldValues)}, New: {System.Text.Json.JsonSerializer.Serialize(newValues)}",
                Timestamp = DateTime.UtcNow
            };
            await _auditRepository.AddAsync(auditLog, CancellationToken.None);
        }

        public async Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync(string? entityName = null, string? entityId = null, int pageNumber = 1, int pageSize = 50)
        {
            var filteredLogs = await _auditRepository.GetPagedAuditLogsAsync(pageNumber, pageSize, entityName, entityId, CancellationToken.None);
            return _mapper.Map<IEnumerable<AuditLogDto>>(filteredLogs.Items);
        }

        public async Task<IEnumerable<AuditLogDto>> GetUserActivityAsync(string userId, int pageNumber = 1, int pageSize = 50)
        {
            var userLogs = await _auditRepository.GetPagedUserLogsAsync(pageNumber, pageSize, userId, CancellationToken.None);
            return _mapper.Map<IEnumerable<AuditLogDto>>(userLogs.Items);
        }

        public async Task<AuditSummaryDto> GetAuditSummaryAsync(DateTime fromDate, DateTime toDate)
        {
            var allLogs = await _auditRepository.GetAllAsync(CancellationToken.None);
            var filteredLogs = allLogs
                .Where(x => x.Timestamp >= fromDate && x.Timestamp <= toDate)
                .ToList();

            return new AuditSummaryDto
            {
                FromDate = fromDate,
                ToDate = toDate,
                TotalActions = filteredLogs.Count,
                UniqueUsers = filteredLogs.Select(x => x.UserId).Distinct().Count(),
                UniqueEntities = filteredLogs.Select(x => x.EntityName).Distinct().Count(),
                ActionCounts = filteredLogs.GroupBy(x => x.Action).ToDictionary(g => g.Key, g => g.Count()),
                EntityCounts = filteredLogs.GroupBy(x => x.EntityName).ToDictionary(g => g.Key, g => g.Count()),
                // ???????? ??: ?userId ???? string ??
                UserActivityCounts = filteredLogs.GroupBy(x => x.UserId).ToDictionary(g => g.Key!, g => g.Count())
            };
        }
    }
}