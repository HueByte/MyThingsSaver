using Core.DTO;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Authentication
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(RegisterDto registerUser);
        Task<VerifiedUserDto> LoginUser(LoginUserDto userDTO);
        Task<VerifiedUserDto> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task ChangePasswordAsync(ChangePasswordDto user);
    }
}