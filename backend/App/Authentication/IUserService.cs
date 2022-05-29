using Core.DTO;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Authentication
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(RegisterDto registerUser);
        Task<VerifiedUserDto> LoginUser(LoginUserDto userDTO, string IpAddress);
        Task ChangePasswordAsync(ChangePasswordDto user);
    }
}