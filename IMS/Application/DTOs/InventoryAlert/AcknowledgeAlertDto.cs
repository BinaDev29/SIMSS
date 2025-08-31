// Application/DTOs/InventoryAlert/AcknowledgeAlertDto.cs
namespace Application.DTOs.InventoryAlert
{
    public class AcknowledgeAlertDto
    {
        public int AlertId { get; set; }
        public string AcknowledgedBy { get; set; } = string.Empty;
        public string? ActionTaken { get; set; }
        public string? Notes { get; set; }
    }
}