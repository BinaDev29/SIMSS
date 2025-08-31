namespace Application.DTOs.Reports
{
    public class ReportSummaryDto
    {
        public int Id { get; set; }
        public string ReportType { get; set; } = string.Empty;
        public string ReportName { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public int GeneratedBy { get; set; }
        public string GeneratedByName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}