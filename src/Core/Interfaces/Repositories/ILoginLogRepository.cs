using MTS.Core.Abstraction;
using MTS.Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface ILoginLogRepository : IIdentityRepository<string, LoginLogModel>
    {

    }
}