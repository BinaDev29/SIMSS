using Application.DTOs.Common;

namespace Application.DTOs.User
{
    public class UserDto : BaseDto
    {
        public required string Username { get; set; }
        public required string Role { get; set; }
    }
}