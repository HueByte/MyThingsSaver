using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Core.Entities.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Options;
using MTS.Core.DTO;
using MTS.Core.Entities;
using MTS.Core.Interfaces.Repositories;
using MTS.Core.Interfaces.Services;
using MTS.Core.Models;

namespace MTS.Core.Services.Authentication
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly JWTOptions _jwtOptions;
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly IJwtAuthentication _jwtAuth;
        public RefreshTokenService(IOptions<JWTOptions> jwtOptions, UserManager<ApplicationUserModel> userManager, IJwtAuthentication jwtAuthentication)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
            _jwtAuth = jwtAuthentication;
        }

        public RefreshTokenModel CreateRefreshToken(string ipAddress)
        {
            var randomSeed = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomSeed);

            return new RefreshTokenModel
            {
                Token = Convert.ToBase64String(randomSeed),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.RefreshTokenExpireTime),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<VerifiedUserDto> RefreshToken(string token, string ipAddress)
        {
            var user = await GetUserByRefreshToken(token);

            // Get matching token
            var refreshToken = user!.RefreshTokens?.FirstOrDefault(e => e.Token == token && e.CreatedByIp == ipAddress);

            if (refreshToken is null || !refreshToken.IsActive)
                throw new Exception("Token is invalid");

            // Get new refresh token and revoke old one
            var newRefreshToken = RotateToken(refreshToken, ipAddress);
            user!.RefreshTokens?.Add(newRefreshToken);

            // Remove old tokens
            RemoveOldRefreshTokens(user);
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var jwtToken = _jwtAuth.GenerateJsonWebToken(user, roles);

            return new VerifiedUserDto
            {
                Username = user.UserName,
                Token = jwtToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpireTime),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.Expires,
                Roles = roles.ToArray(),
                AvatarUrl = user.AvatarUrl
            };
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token is invalid");

            var user = await GetUserByRefreshToken(token);
            var refreshToken = user!.RefreshTokens?.FirstOrDefault(e => e.Token == token);

            if (refreshToken is null || !refreshToken.IsActive)
                throw new Exception("Token is invalid");

            RevokeRefreshToken(refreshToken, ipAddress);

            await _userManager.UpdateAsync(user);
        }

        /// <summary>
        /// removes old refresh tokens for user based on RefreshTokenExpireTime setting 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void RemoveOldRefreshTokens(ApplicationUserModel user)
        {
            user.RefreshTokens?.RemoveAll(token => !token.IsActive
                                      && token.Created.AddDays(_jwtOptions.RefreshTokenExpireTime) <= DateTime.UtcNow);
        }

        /// <summary>
        /// revokes refresh token with given reason
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ipAddress"></param>
        /// <param name="reason"></param>
        private static void RevokeRefreshToken(RefreshTokenModel token, string ipAddress, string? reason = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
        }

        /// <summary>
        /// Expires old refresh token and returns a new one
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private RefreshTokenModel RotateToken(RefreshTokenModel token, string ipAddress)
        {
            var newRefreshToken = CreateRefreshToken(ipAddress);
            RevokeRefreshToken(token, ipAddress, "Rotated Token");
            return newRefreshToken;
        }

        /// <summary>
        /// fetches user by given refresh token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HandledException"></exception>
        private async Task<ApplicationUserModel> GetUserByRefreshToken(string token)
        {
            var user = await _userManager.Users.Include(e => e.RefreshTokens)
                                               .SingleOrDefaultAsync(user => user.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
                throw new Exception("Token is invalid");

            return user;
        }
    }
}