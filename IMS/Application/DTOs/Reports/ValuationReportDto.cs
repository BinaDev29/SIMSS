namespace Application.DTOs.Reports
{
    public class ValuationReportDto
    {
        public DateTime AsOfDate { get; set; }
        public string ValuationMethod { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }
        public int TotalItems { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<ItemValuationDto> Items { get; set; } = [];
    }

    public class ItemValuationDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string GodownName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitValue { get; set; }
        public decimal TotalValue { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}