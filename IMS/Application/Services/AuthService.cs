// Application/Services/AuthService.cs
using Application.Contracts;
using Application.DTOs.Auth;
using Application.DTOs.User;
using Application.Exceptions;
using Application.Services;
using AutoMapper;
using BCrypt.Net;
using Domain.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            IUserRepository userRepository,
            IMapper mapper,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username, cancellationToken);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new ValidationException("Invalid username or password");
            }

            if (!user.IsActive)
            {
                throw new ValidationException("Account is deactivated");
            }

            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user, cancellationToken);

            var token = _jwtTokenService.GenerateToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddHours(24),
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
        {
            if (await _userRepository.GetUserByUsernameAsync(createUserDto.Username) != null)
            {
                throw new ValidationException("Username already exists");
            }

            if (await _userRepository.GetUserByEmailAsync(createUserDto.Email) != null)
            {
                throw new ValidationException("Email already exists");
            }

            var user = new User
            {
                Username = createUserDto.Username,
                Email = createUserDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.PasswordHash),
                Role = createUserDto.Role,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var createdUser = await _userRepository.AddAsync(user, cancellationToken);

            var token = _jwtTokenService.GenerateToken(createdUser);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            return new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddHours(24),
                UserId = createdUser.Id,
                Username = createdUser.Username,
                Email = createdUser.Email,
                Role = createdUser.Role,
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            throw new ValidationException("Invalid refresh token");
        }

        public async Task<bool> LogoutAsync(string token, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User", userId);
            }

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                throw new ValidationException("Current password is incorrect");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.LastModifiedDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user, cancellationToken);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);

            if (user == null)
            {
                return true;
            }

            await Task.CompletedTask;
            return true;
        }
    }
}