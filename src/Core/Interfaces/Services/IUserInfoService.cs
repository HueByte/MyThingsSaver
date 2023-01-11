using Core.DTO;

namespace Core.Interfaces.Services
{
    public interface IUserInfoService
    {
        Task<UserInfoDto> GetUserInfoAsync();
    }
}