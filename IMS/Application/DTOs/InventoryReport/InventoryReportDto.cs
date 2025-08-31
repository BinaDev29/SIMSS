// Application/DTOs/InventoryReport/InventoryReportDto.cs
using Application.DTOs.Common;

namespace Application.DTOs.InventoryReport
{
    public class InventoryReportDto : BaseDto
    {
        public string ReportName { get; set; } = string.Empty;
        public string ReportType { get; set; } = string.Empty;
        public DateTime GeneratedDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Parameters { get; set; }
        public string? ReportData { get; set; }
        public string GeneratedBy { get; set; } = string.Empty;
        public string? FilePath { get; set; }
        public string? Status { get; set; }
    }
}