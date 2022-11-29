using Core.Abstraction;
using Core.Models;
using Core.Services.CurrentUser;

namespace Infrastructure.Repositories;

public class EntryRepository : IdentityBaseRepository<string, EntryModel, AppDbContext>
{
    public EntryRepository(AppDbContext context, ICurrentUserService currentUser) : base(context, currentUser) { }
}