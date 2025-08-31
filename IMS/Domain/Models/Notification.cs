using Domain.Common;
using System;

namespace Domain.Models
{
    public class Notification : BaseDomainEntity
    {
        public required string Title { get; set; }
        public required string Message { get; set; }
        public required string Type { get; set; } // "Info", "Warning", "Error", "Success"
        public required string Priority { get; set; } // "Low", "Medium", "High", "Critical"
        public required int UserId { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }
        public string? ActionUrl { get; set; }
        public string? Metadata { get; set; } // JSON string for additional data
        
        // Navigation properties
        public User User { get; set; } = null!;
    }
}