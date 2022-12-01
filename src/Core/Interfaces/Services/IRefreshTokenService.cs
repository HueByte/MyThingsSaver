using System.Threading.Tasks;
using Core.DTO;
using Core.Models;

namespace Core.Interfaces.Services;

public interface IRefreshTokenService
{
    RefreshTokenModel CreateRefreshToken(string ipAddress);
    Task<VerifiedUserDto> RefreshToken(string token, string ipAddress);
    Task RevokeToken(string token, string ipAddress);
    Task RemoveOldRefreshTokens(ApplicationUserModel user);
}