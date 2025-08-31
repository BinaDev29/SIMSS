// Application/DTOs/Notification/NotificationDto.cs
using Application.DTOs.Common;

namespace Application.DTOs.Notification
{
    public class NotificationDto : BaseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public string? ActionUrl { get; set; }
        public string? Metadata { get; set; }
    }
}