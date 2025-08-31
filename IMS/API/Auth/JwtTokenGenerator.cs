using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Auth
{
    public class JwtTokenGenerator
    {
        private static readonly JwtSecurityTokenHandler _tokenHandler = new();
        private readonly string _jwtKey;
        private readonly string? _jwtIssuer;
        private readonly string? _jwtAudience;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            _jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
        }

        public string GenerateToken(string username, string role)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role cannot be null or empty.", nameof(role));

            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, username),
                new(ClaimTypes.Name, username),
                new(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _jwtIssuer,
                _jwtAudience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            return _tokenHandler.WriteToken(token);
        }
    }
}