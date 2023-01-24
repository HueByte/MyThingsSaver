using MTS.Core.Abstraction;
using MTS.Core.Interfaces.Repositories;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;
using MTS.Infrastructure;

namespace Infrastructure.Repositories
{
    public class PublicEntryRepository : IdentityBaseRepository<int, PublicEntryModel, MTSContext>, IPublicEntryRepository
    {
        public PublicEntryRepository(MTSContext context, ICurrentUserService currentUser) : base(context, currentUser) { }
    }
}