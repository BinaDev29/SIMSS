// Application/DTOs/InventoryReport/CreateInventoryReportDto.cs
namespace Application.DTOs.InventoryReport
{
    public class CreateInventoryReportDto
    {
        public string ReportName { get; set; } = string.Empty;
        public string ReportType { get; set; } = string.Empty;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Parameters { get; set; }
        public string GeneratedBy { get; set; } = string.Empty;
    }
}