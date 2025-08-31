namespace Application.DTOs.Reports
{
    public class StockReportParametersDto
    {
        public int? GodownId { get; set; }
        public int? ItemId { get; set; }
        public string? Category { get; set; }
        public bool IncludeLowStock { get; set; } = true;
        public bool IncludeOutOfStock { get; set; } = true;
        public DateTime? AsOfDate { get; set; }
        public int FromDate { get; set; }
        public int ToDate { get; set; }
    }
}