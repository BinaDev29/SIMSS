namespace Application.DTOs.Audit
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public object? OldValues { get; set; }
        public object? NewValues { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}