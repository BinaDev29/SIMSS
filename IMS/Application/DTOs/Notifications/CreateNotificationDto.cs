// Application/DTOs/Notifications/CreateNotificationDto.cs
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Notifications
{
    public class CreateNotificationDto
    {
        [Required]
        public string Message { get; set; } = string.Empty;
        
        [Required]
        public int UserId { get; set; }
        
        public string? Type { get; set; }
        public string? Title { get; set; }
    }
}