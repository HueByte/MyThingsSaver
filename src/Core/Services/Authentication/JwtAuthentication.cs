using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MTS.Core.Entities;
using MTS.Core.Interfaces.Services;
using MTS.Core.Models;

namespace MTS.Core.Services.Authentication
{
    public class JwtAuthentication : IJwtAuthentication
    {
        private readonly AppSettingsRoot _settings;
        public JwtAuthentication(AppSettingsRoot configuration)
        {
            _settings = configuration;
        }

        // TODO: consider email/username choice system configurable
        public string GenerateJsonWebToken(ApplicationUserModel user, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JWT.Key));
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(_settings.JWT.AccessTokenExpireTime),
                issuer: _settings.JWT.Issuer,
                audience: _settings.JWT.Audience,
                claims: claims,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}