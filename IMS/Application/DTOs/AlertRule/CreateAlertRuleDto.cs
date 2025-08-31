// Application/DTOs/AlertRule/CreateAlertRuleDto.cs
namespace Application.DTOs.AlertRule
{
    public class CreateAlertRuleDto
    {
        public string RuleName { get; set; } = string.Empty;
        public string RuleType { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public string AlertMessage { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string? NotificationChannels { get; set; }
        public string? Recipients { get; set; }
    }
}