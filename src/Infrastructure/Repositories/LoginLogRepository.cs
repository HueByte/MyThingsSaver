using Core.Interfaces.Repositories;
using MTS.Core.Abstraction;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;
using MTS.Infrastructure;

namespace Infrastructure.Repositories
{
    public class LoginLogRepository : IdentityBaseRepository<string, LoginLogModel, MTSContext>, ILoginLogRepository
    {
        public LoginLogRepository(MTSContext context, ICurrentUserService currentUser) : base(context, currentUser) { }
    }
}