using MTS.Core.Abstraction;
using MTS.Core.Interfaces.Repositories;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;

namespace MTS.Infrastructure.Repositories;

public class EntryRepository : IdentityBaseRepository<string, EntryModel, MTSContext>, IEntryRepository
{
    public EntryRepository(MTSContext context, ICurrentUserService currentUser) : base(context, currentUser) { }
}