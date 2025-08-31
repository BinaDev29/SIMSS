// Application/DTOs/InventoryAlert/UpdateInventoryAlertDto.cs
namespace Application.DTOs.InventoryAlert
{
    public class UpdateInventoryAlertDto
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public bool? IsActive { get; set; }
        public string? ActionTaken { get; set; }
        public string? Notes { get; set; }
    }
}