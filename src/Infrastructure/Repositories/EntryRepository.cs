using Core.Abstraction;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Services.CurrentUser;

namespace Infrastructure.Repositories;

public class EntryRepository : IdentityBaseRepository<string, EntryModel, AppDbContext>, IEntryRepository
{
    public EntryRepository(AppDbContext context, ICurrentUserService currentUser) : base(context, currentUser) { }
}