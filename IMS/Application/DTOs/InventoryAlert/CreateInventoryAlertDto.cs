// Application/DTOs/InventoryAlert/CreateInventoryAlertDto.cs
namespace Application.DTOs.InventoryAlert
{
    public class CreateInventoryAlertDto
    {
        public int ItemId { get; set; }
        public int GodownId { get; set; }
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime AlertDate { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal? ThresholdValue { get; set; }
        public decimal? RecommendedAction { get; set; }
        public decimal ConfidenceScore { get; set; } = 1.0m;
        public string? Notes { get; set; }
    }
}