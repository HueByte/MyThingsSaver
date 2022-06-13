using Core.DTO;
using Core.Models;

namespace App.Authentication
{
    public interface IRefreshTokenService
    {
        RefreshToken CreateRefreshToken(string ipAddress);
        Task<VerifiedUserDto> RefreshToken(string token, string ipAddress);
        Task RevokeToken(string token, string ipAddress);
        Task RemoveOldRefreshTokens(ApplicationUser user);
    }
}