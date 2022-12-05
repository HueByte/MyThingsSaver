using System.Threading.Tasks;
using MTS.Core.DTO;
using MTS.Core.Models;

namespace MTS.Core.Interfaces.Services;

public interface IRefreshTokenService
{
    RefreshTokenModel CreateRefreshToken(string ipAddress);
    Task<VerifiedUserDto> RefreshToken(string token, string ipAddress);
    Task RevokeToken(string token, string ipAddress);
    Task RemoveOldRefreshTokens(ApplicationUserModel user);
}