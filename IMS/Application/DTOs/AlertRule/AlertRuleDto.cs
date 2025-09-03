namespace Application.DTOs.AlertRule
{
    public record AlertRuleDto
    {
        public int Id { get; init; }
        public string RuleName { get; init; } = string.Empty;
        public string RuleType { get; init; } = string.Empty;
        public string Condition { get; init; } = string.Empty;
        public string AlertMessage { get; init; } = string.Empty;
        public string? Priority { get; init; }
        public bool IsActive { get; init; }
        public string? NotificationChannels { get; init; }
        public string? Recipients { get; init; }
        public DateTime? LastTriggered { get; init; }
        public int TriggerCount { get; init; }
    }
}