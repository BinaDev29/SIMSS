namespace Application.DTOs.Audit
{
    public class AuditSummaryDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalActions { get; set; }
        public int UniqueUsers { get; set; }
        public int UniqueEntities { get; set; }
        public Dictionary<string, int> ActionCounts { get; set; } = [];
        public Dictionary<string, int> EntityCounts { get; set; } = [];
        public Dictionary<string, int> UserActivityCounts { get; set; } = [];
    }
}