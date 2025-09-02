using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MiniDiscord.IdentityService.Models;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using BuildingBlocks.Jwt;

namespace MiniDiscord.IdentityService.Services
{
    public class JwtGenerator(IOptions<JwtOptions> opt)
        : IJwtGenerator
    {
        private readonly JwtOptions _opt = opt.Value;
        private readonly SymmetricSecurityKey _signingKey =
            new(Encoding.UTF8.GetBytes(opt.Value.Key));

        public AuthResponse Generate(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

            var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_opt.ExpMinutes);

            var token = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: _opt.Issuer,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthResponse(jwt, expires);
        }
    }

}
