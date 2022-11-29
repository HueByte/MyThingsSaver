using Core.Abstraction;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface IEntryRepository : IIdentityRepository<string, EntryModel>
    {

    }
}