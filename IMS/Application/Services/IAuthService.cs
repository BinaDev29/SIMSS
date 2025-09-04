using Application.DTOs.Auth;
using Application.DTOs.User;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> RegisterAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> LogoutAsync(string token, CancellationToken cancellationToken = default);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
        Task<bool> ResetPasswordAsync(string email, CancellationToken cancellationToken = default);
    }
}