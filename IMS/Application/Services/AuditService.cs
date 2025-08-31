using Application.Contracts;
using Application.Services;
using Application.DTOs.Audit;
using Domain.Models;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuditService(IGenericRepository<AuditLog> auditRepository, IMapper mapper) : IAuditService
    {
        public async Task LogAsync(string entityName, string entityId, string action, object? oldValues, object? newValues, string userId, string userName)
        {
            var auditLog = new AuditLog
            {
                EntityName = entityName,
                EntityId = int.TryParse(entityId, out var id) ? id : 0,
                Action = action,
                UserId = userId,
                Details = $"Old: {System.Text.Json.JsonSerializer.Serialize(oldValues)}, New: {System.Text.Json.JsonSerializer.Serialize(newValues)}",
                Timestamp = DateTime.UtcNow
            };

            await auditRepository.AddAsync(auditLog, CancellationToken.None);
        }

        public async Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync(string? entityName = null, string? entityId = null, int pageNumber = 1, int pageSize = 50)
        {
            var allLogs = await auditRepository.GetAllAsync(CancellationToken.None);
            var filteredLogs = allLogs.AsQueryable();

            if (!string.IsNullOrEmpty(entityName))
                filteredLogs = filteredLogs.Where(x => x.EntityName == entityName);

            if (!string.IsNullOrEmpty(entityId) && int.TryParse(entityId, out var id))
                filteredLogs = filteredLogs.Where(x => x.EntityId == id);

            var pagedLogs = filteredLogs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return mapper.Map<IEnumerable<AuditLogDto>>(pagedLogs);
        }

        public async Task<IEnumerable<AuditLogDto>> GetUserActivityAsync(string userId, int pageNumber = 1, int pageSize = 50)
        {
            var allLogs = await auditRepository.GetAllAsync(CancellationToken.None);
            var userLogs = allLogs
                .Where(x => x.UserId == userId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return mapper.Map<IEnumerable<AuditLogDto>>(userLogs);
        }

        public async Task<AuditSummaryDto> GetAuditSummaryAsync(DateTime fromDate, DateTime toDate)
        {
            var allLogs = await auditRepository.GetAllAsync(CancellationToken.None);
            var filteredLogs = allLogs.Where(x => x.Timestamp >= fromDate && x.Timestamp <= toDate).ToList();

            return new AuditSummaryDto
            {
                FromDate = fromDate,
                ToDate = toDate,
                TotalActions = filteredLogs.Count,
                UniqueUsers = filteredLogs.Select(x => x.UserId).Distinct().Count(),
                UniqueEntities = filteredLogs.Select(x => x.EntityName).Distinct().Count(),
                ActionCounts = filteredLogs.GroupBy(x => x.Action).ToDictionary(g => g.Key, g => g.Count()),
                EntityCounts = filteredLogs.GroupBy(x => x.EntityName).ToDictionary(g => g.Key, g => g.Count()),
                UserActivityCounts = filteredLogs.GroupBy(x => x.UserId).ToDictionary(g => g.Key, g => g.Count())
            };
        }
    }
}