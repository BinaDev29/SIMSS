using Domain.Common;
using System;

namespace Domain.Models
{
    public class Notification : BaseDomainEntity
    {
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required string Type { get; set; } // "Info", "Warning", "Error", "Success"
        public required int Priority { get; set; } // 1=Low, 2=Medium, 3=High, 4=Critical
        public required int UserId { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }
        public string? ActionUrl { get; set; }
        public string? Metadata { get; set; } // JSON string for additional data
        
        // Navigation properties
        public User User { get; set; } = null!;

        // Add backward compatibility property
        public DateTime CreatedAt => CreatedDate;
    }
}