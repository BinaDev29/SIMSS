using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AlertRule
{
    public record CreateAlertRuleDto
    {
        [Required(ErrorMessage = "Rule name is required.")]
        [StringLength(100, ErrorMessage = "Rule name cannot exceed 100 characters.")]
        public string RuleName { get; init; } = string.Empty;

        [Required(ErrorMessage = "Rule type is required.")]
        [StringLength(50, ErrorMessage = "Rule type cannot exceed 50 characters.")]
        public string RuleType { get; init; } = string.Empty;

        [Required(ErrorMessage = "Condition is required.")]
        public string Condition { get; init; } = string.Empty;

        [Required(ErrorMessage = "Alert message is required.")]
        public string AlertMessage { get; init; } = string.Empty;

        [StringLength(20)]
        public string? Priority { get; init; }

        public bool IsActive { get; init; } = true;

        public string? NotificationChannels { get; init; }

        public string? Recipients { get; init; }
    }
}