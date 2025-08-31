// Application/DTOs/AuditLog/AuditLogDto.cs
using Application.DTOs.Common;

namespace Application.DTOs.AuditLog
{
    public class AuditLogDto : BaseDto
    {
        public string EntityName { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}