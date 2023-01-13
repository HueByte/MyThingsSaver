using Core.DTO;
using Microsoft.AspNetCore.Identity;
using MTS.Core.DTO;

namespace MTS.Core.Interfaces.Services;

public interface IUserService
{
    Task<IdentityResult> CreateUser(RegisterDto registerUser);
    Task<VerifiedUserDto> LoginUser(LoginUserDto userDTO, string IpAddress);
    Task<UserInfoDto> GetUserInfoAsync();
    Task<bool> ChangeUserAvatarAsync(string avatarUrl);
    Task<bool> ChangeUsernameAsync(string username);
    Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
    Task<bool> ChangeEmailAsync(string email);
}