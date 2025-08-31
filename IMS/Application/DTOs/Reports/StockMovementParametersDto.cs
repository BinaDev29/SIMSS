namespace Application.DTOs.Reports
{
    public class StockMovementParametersDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? ItemId { get; set; }
        public int? GodownId { get; set; }
        public string? MovementType { get; set; } // "Inward", "Outward", "All"
        public int FromDate { get; set; }
        public int ToDate { get; set; }
    }
}