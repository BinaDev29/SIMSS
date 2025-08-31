using Domain.Common;
using System;

namespace Domain.Models
{
    public class AlertRule : BaseDomainEntity
    {
        public required string RuleName { get; set; }
        public required string RuleType { get; set; } // "LowStock", "HighStock", "ExpiryDate", "Custom"
        public required string Condition { get; set; } // JSON string for rule conditions
        public required string AlertMessage { get; set; }
        public required string Priority { get; set; } // "Low", "Medium", "High", "Critical"
        public bool IsActive { get; set; } = true;
        public DateTime? LastTriggered { get; set; }
        public int TriggerCount { get; set; } = 0;
        public string? NotificationChannels { get; set; } // JSON array of channels: "Email", "SMS", "InApp"
        public string? Recipients { get; set; } // JSON array of user IDs or email addresses
    }
}