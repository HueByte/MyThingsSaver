using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Core.Entities.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MTS.Core.Entities;
using MTS.Core.Interfaces.Services;
using MTS.Core.Models;

namespace MTS.Core.Services.Authentication
{
    public class JwtAuthentication : IJwtAuthentication
    {
        private readonly JWTOptions _jwtOptions;
        public JwtAuthentication(IOptions<JWTOptions> options)
        {
            _jwtOptions = options.Value;
        }

        // TODO: consider email/username choice system configurable
        public string GenerateJsonWebToken(ApplicationUserModel user, IList<string> roles)
        {
            if (user is null)
                throw new ArgumentNullException($"User is null");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpireTime),
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}