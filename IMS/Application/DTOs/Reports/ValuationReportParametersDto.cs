namespace Application.DTOs.Reports
{
    public class ValuationReportParametersDto
    {
        public DateTime AsOfDate { get; set; }
        public int? GodownId { get; set; }
        public string? Category { get; set; }
        public string? ValuationMethod { get; set; } = "FIFO"; // "FIFO", "LIFO", "Average"
    }
}