// Application/DTOs/Notification/CreateNotificationDto.cs
namespace Application.DTOs.Notification
{
    public class CreateNotificationDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string? ActionUrl { get; set; }
        public string? Metadata { get; set; }
    }
}