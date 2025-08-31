namespace Application.DTOs.Reports
{
    public class ItemMovementDto
    {
        public DateTime Date { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Reference { get; set; } = string.Empty;
    }
}