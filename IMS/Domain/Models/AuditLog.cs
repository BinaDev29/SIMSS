using Domain.Common;
using System;

namespace Domain.Models
{
    public class AuditLog : BaseDomainEntity
    {

        public required string EntityName { get; set; }
        public required string EntityId { get; set; }
        public required string Action { get; set; } // "Create", "Update", "Delete"
        public string? OldValues { get; set; } // JSON string
        public string? NewValues { get; set; } // JSON string
        public required int UserId { get; set; } // Adjusted to int
        public required string UserName { get; set; }
        public required DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public required string Details { get; set; }
    }
}