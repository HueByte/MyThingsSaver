using MTS.Core.Models;

namespace Core.Interfaces.Services
{
    public interface ILoginLogService
    {
        Task LogLoginAsync(ApplicationUserModel user, string ipAddress);
        Task<List<LoginLogModel>> GetAllLoginLogsAsync();
    }
}