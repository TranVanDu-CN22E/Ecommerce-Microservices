using IdentityService.Application.Abstractions.Services;
using IdentityService.Domain.Aggregates.RoleAggregate;
using IdentityService.Domain.Aggregates.UserAggregate;
using Medo;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenService
    {
        private readonly IConfiguration _config;
        public JwtTokenGenerator(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateAccessToken(User user, List<RoleId>? roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
                new(JwtRegisteredClaimNames.Jti, Uuid7.NewUuid7().ToString())
            };
            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Value.ToString()));
                }
            }
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        int.Parse(_config["Jwt:AccessTokenExpireMinutes"]??"10")
                    ),
                    signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }

        public Task<Guid?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
