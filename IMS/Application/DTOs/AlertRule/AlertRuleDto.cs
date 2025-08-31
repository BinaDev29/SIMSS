// Application/DTOs/AlertRule/AlertRuleDto.cs
using Application.DTOs.Common;

namespace Application.DTOs.AlertRule
{
    public class AlertRuleDto : BaseDto
    {
        public string RuleName { get; set; } = string.Empty;
        public string RuleType { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public string AlertMessage { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? LastTriggered { get; set; }
        public int TriggerCount { get; set; }
        public string? NotificationChannels { get; set; }
        public string? Recipients { get; set; }
    }
}