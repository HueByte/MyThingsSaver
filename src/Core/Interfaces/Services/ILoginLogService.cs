using Core.DTO;
using MTS.Core.Models;

namespace Core.Interfaces.Services
{
    public interface ILoginLogService
    {
        Task AddLoginLogAsync(ApplicationUserModel user, string ipAddress);
        Task<List<LoginLogModel>> GetAllUserLoginLogsAsync();
        Task<List<LoginLogsDto>> GetLoginLogsPaginatedAsync(int page, int pageSize);
        Task<List<LoginLogModel>> GetUserLoginLogsPaginatedAsync(int page, int pageSize);
        Task<int> GetLoginLogsCountAsync();
        Task<int> GetUserLoginLogsCountAsync();
    }
}