using Core.Abstraction;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Services.CurrentUser;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : BaseRepository<int, RefreshTokenModel, AppDbContext>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext context) : base(context) { }
    }
}