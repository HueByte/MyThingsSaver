using Core.Abstraction;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Services.CurrentUser;

namespace Infrastructure.Repositories;

public class EntryRepository : IdentityBaseRepository<string, EntryModel, MTSContext>, IEntryRepository
{
    public EntryRepository(MTSContext context, ICurrentUserService currentUser) : base(context, currentUser) { }
}