using Core.DTO;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Authentication
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(RegisterDto registerUser);
        Task<VerifiedUser> LoginUser(LoginUserDto userDTO);
        Task<VerifiedUser> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task ChangePasswordAsync(ChangePasswordDto user);
    }
}