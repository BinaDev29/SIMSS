namespace Application.DTOs.Reports
{
    public class UserActivityReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public DateTime GeneratedAt { get; set; }
        public List<UserReportDto> Users { get; set; } = [];
    }

    public class UserReportDto
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}