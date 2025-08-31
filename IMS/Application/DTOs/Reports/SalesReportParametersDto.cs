namespace Application.DTOs.Reports
{
    public class SalesReportParametersDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? CustomerId { get; set; }
        public int? ItemId { get; set; }
        public int? GodownId { get; set; }
        public string? GroupBy { get; set; } // "Daily", "Weekly", "Monthly"
        public int FromDate { get; set; }
        public int ToDate { get; set; }
    }
}