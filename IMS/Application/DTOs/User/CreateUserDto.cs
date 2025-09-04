using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class CreateUserDto
    {
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
        public string? Email { get; internal set; }
    }
}