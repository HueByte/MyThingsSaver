using Core.DTO;

namespace Core.Interfaces.Services
{
    public interface IUserInfoService
    {
        Task<UserInfoDto> GetUserInfoAsync();
        Task<bool> ChangeUserAvatarAsync(string avatarUrl);
        Task<bool> ChangeUsernameAsync(string username);
    }
}