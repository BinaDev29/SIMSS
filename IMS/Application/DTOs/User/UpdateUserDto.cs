using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class UpdateUserDto
    {
        public required int Id { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
    }
}