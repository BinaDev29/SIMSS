using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Notifications
{
    public class CreateNotificationDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Message { get; set; } = string.Empty;
        
        public string Type { get; set; } = "Info";
        
        [Required]
        public int UserId { get; set; }
    }
}