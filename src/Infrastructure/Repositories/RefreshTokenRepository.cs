using MTS.Core.Abstraction;
using MTS.Core.Interfaces.Repositories;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;

namespace MTS.Infrastructure.Repositories
{
    public class RefreshTokenRepository : BaseRepository<int, RefreshTokenModel, MTSContext>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(MTSContext context) : base(context) { }
    }
}