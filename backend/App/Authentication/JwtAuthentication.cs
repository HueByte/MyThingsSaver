using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core.Entities;
using Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace App.Authentication
{
    public class JwtAuthentication : IJwtAuthentication
    {
        private readonly AppSettingsRoot _configuration;
        public JwtAuthentication(AppSettingsRoot configuration)
        {
            _configuration = configuration;
        }

        // TODO: consider email/username choice system configurable
        public string GenerateJsonWebToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.JWT.Key));
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(90),
                issuer: _configuration.JWT.Issuer,
                audience: _configuration.JWT.Audience,
                claims: claims,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken CreateRefreshToken()
        {
            var randomSeed = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomSeed);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomSeed),
                Expires = DateTime.UtcNow.AddDays(5),
                Created = DateTime.UtcNow
            };
        }
    }
}