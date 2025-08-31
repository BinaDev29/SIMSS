// Application/DTOs/InventoryAlert/InventoryAlertDto.cs
using Application.DTOs.Common;

namespace Application.DTOs.InventoryAlert
{
    public class InventoryAlertDto : BaseDto
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int GodownId { get; set; }
        public string? GodownName { get; set; }
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime AlertDate { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal? ThresholdValue { get; set; }
        public decimal? RecommendedAction { get; set; }
        public bool IsActive { get; set; }
        public bool IsAcknowledged { get; set; }
        public DateTime? AcknowledgedDate { get; set; }
        public string? AcknowledgedBy { get; set; }
        public decimal ConfidenceScore { get; set; }
        public string? ActionTaken { get; set; }
        public string? Notes { get; set; }
    }
}