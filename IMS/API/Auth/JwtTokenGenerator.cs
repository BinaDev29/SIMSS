using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Auth
{
    public class JwtTokenGenerator(IConfiguration configuration)
    {
        private static readonly JwtSecurityTokenHandler _tokenHandler = new();
        private readonly string _jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is not configured.");
        private readonly string _jwtIssuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is not configured.");
        private readonly string _jwtAudience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience is not configured.");

        public string GenerateToken(string username, string role)
        {
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