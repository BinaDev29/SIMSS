// Application/DTOs/Notifications/NotificationDto.cs
namespace Application.DTOs.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Type { get; set; }
        public string? Title { get; set; }
    }
}