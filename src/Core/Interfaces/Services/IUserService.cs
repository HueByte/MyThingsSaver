using Core.DTO;
using Microsoft.AspNetCore.Identity;
using MTS.Core.DTO;

namespace MTS.Core.Interfaces.Services;

public interface IUserService
{
    Task<List<ManagementUserDto>> SearchUserAsync(string query);
    Task<IdentityResult> CreateUser(RegisterDto registerUser);
    Task<VerifiedUserDto> LoginUser(LoginUserDto userDTO, string IpAddress);
    Task<UserInfoDto> GetUserInfoByEmailAsync(string email);
    Task<UserInfoDto> GetUserInfoByUsernameAsync(string username);
    Task<UserInfoDto> GetUserInfoByIdAsync(string id);
    Task<List<ManagementUserDto>> GetManagementUsers();
    Task<bool> ChangeUserAvatarAsync(string avatarUrl);
    Task<bool> ChangeUsernameAsync(string username, string password);
    Task<bool> ChangePasswordAsync(string currentPassword, string newPassword);
    Task<bool> ChangeEmailAsync(string email, string password);
}