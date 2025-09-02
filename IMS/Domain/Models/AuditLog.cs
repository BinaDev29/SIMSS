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
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public required string Details { get; set; }

        // Sample method to demonstrate correct initialization
        public static AuditLog CreateAuditLog(int userId, string userName, DateTime timestamp, string action, string entityName, string entityId, string details, string? oldValues = null, string? newValues = null, string? ipAddress = null, string? userAgent = null)
        {
            // Ensure that userId and other int parameters are correctly converted to string if necessary
            return new AuditLog
            {
                UserId = userId.ToString(), // Convert int to string
                UserName = userName,
                Timestamp = timestamp,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Details = details,
                OldValues = oldValues,
                NewValues = newValues,
                IpAddress = ipAddress,
                UserAgent = userAgent
            };
        }
    }
}