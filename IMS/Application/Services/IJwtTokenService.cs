using Domain.Models;

namespace Application.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
        int GetUserIdFromToken(string token);
    }
}