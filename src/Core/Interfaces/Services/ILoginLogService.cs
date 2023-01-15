using MTS.Core.Models;

namespace Core.Interfaces.Services
{
    public interface ILoginLogService
    {
        Task LogLoginAsync(ApplicationUserModel user, string ipAddress);
        Task<List<LoginLogModel>> GetAllLoginLogsAsync();
        Task<List<LoginLogModel>> GetLoginLogsPaginatedAsync(int page, int pageSize);
        Task<List<LoginLogModel>> GetUserLoginLogsPaginatedAsync(int page, int pageSize);
        Task<int> GetLoginLogsCountAsync();
        Task<int> GetUserLoginLogsCountAsync();
    }
}