using System.Security.Cryptography;
using Core.DTO;
using Core.Entities;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace App.Authentication
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private AppSettingsRoot _settings;
        private AppDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IJwtAuthentication _jwtAuth;
        public RefreshTokenService(AppDbContext context, AppSettingsRoot settings, UserManager<ApplicationUser> userManager, IJwtAuthentication jwtAuthentication)
        {
            _context = context;
            _settings = settings;
            _userManager = userManager;
            _jwtAuth = jwtAuthentication;
        }

        public RefreshToken CreateRefreshToken(string ipAddress)
        {

            var randomSeed = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomSeed);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomSeed),
                Expires = DateTime.UtcNow.AddMinutes(_settings.JWT.RefreshTokenExpireTime),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task<VerifiedUserDto> RefreshToken(string token, string ipAddress)
        {
            var user = await GetUserByRefreshToken(token);

            // Fetch matching token
            var refreshToken = user.RefreshTokens.FirstOrDefault(e => e.Token == token && e.CreatedByIp == ipAddress);

            if (refreshToken is null || !refreshToken.IsActive)
                throw new EndpointException("Token is invalid");

            // Get new refresh token and revoke old one
            var newRefreshToken = RotateToken(refreshToken, ipAddress);
            user.RefreshTokens.Add(newRefreshToken);

            // Remove old tokens
            await RemoveOldRefreshTokens(user);

            _context.Update(user);
            await _context.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var jwtToken = _jwtAuth.GenerateJsonWebToken(user, roles);

            return new VerifiedUserDto
            {
                Username = user.UserName,
                Token = jwtToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_settings.JWT.AccessTokenExpireTime),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.Expires,
                Roles = roles.ToArray()
            };
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception("Token is invalid");

            var user = await GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.FirstOrDefault(e => e.Token == token);

            if (refreshToken is null || !refreshToken.IsActive)
                throw new Exception("Token is invalid");

            RevokeRefreshToken(refreshToken, ipAddress);

            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// removes old refresh tokens for user based on RefreshTokenExpireTime setting 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task RemoveOldRefreshTokens(ApplicationUser user)
        {
            user.RefreshTokens.RemoveAll(token => !token.IsActive
                                      && token.Created.AddDays(_settings.JWT.RefreshTokenExpireTime) <= DateTime.UtcNow);

            return Task.CompletedTask;
        }

        /// <summary>
        /// revokes refresh token with given reason
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ipAddress"></param>
        /// <param name="reason"></param>
        private static void RevokeRefreshToken(RefreshToken token, string ipAddress, string? reason = null)
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
        private RefreshToken RotateToken(RefreshToken token, string ipAddress)
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
        /// <exception cref="EndpointException"></exception>
        private async Task<ApplicationUser> GetUserByRefreshToken(string token)
        {
            var user = await _userManager.Users.Include(e => e.RefreshTokens)
                                               .SingleOrDefaultAsync(user => user.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
                throw new EndpointException("Token is invalid");

            return user;
        }
    }
}