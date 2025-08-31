namespace Application.DTOs.Reports
{
    public class StockMovementReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalMovements { get; set; }
        public int InwardMovements { get; set; }
        public int OutwardMovements { get; set; }
        public decimal TotalInwardValue { get; set; }
        public decimal TotalOutwardValue { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<MovementDetailDto> Movements { get; set; } = [];
    }

    public class MovementDetailDto
    {
        public DateTime Date { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string GodownName { get; set; } = string.Empty;
        public string MovementType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalValue { get; set; }
        public string Reference { get; set; } = string.Empty;
    }
}