using Core.Abstraction;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Services.CurrentUser;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : BaseRepository<int, RefreshTokenModel, MTSContext>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(MTSContext context) : base(context) { }
    }
}