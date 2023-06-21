using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Models;

namespace VolunteerWorkApi.Services.Jwts
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthToken CreateToken(IdentityUser<long> user, string role)
        {
            var expiration = DateTime.UtcNow
                .AddHours(ApiConstants.TokenExpirationHours);

            var token = CreateJwtToken(
                CreateClaims(user, role),
                CreateSigningCredentials(),
                expiration
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return new AuthToken(
                tokenHandler.WriteToken(token),
                expiration);
        }

        private static JwtSecurityToken CreateJwtToken(
            Claim[] claims, SigningCredentials credentials,
            DateTime expiration) =>
            new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private static Claim[] CreateClaims(
            IdentityUser<long> user, string role) =>
            new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!), 
                new Claim(ClaimTypes.Role, role)
            };

        private SigningCredentials CreateSigningCredentials() =>
            new(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                ),
                SecurityAlgorithms.HmacSha256
            );
    }
}
