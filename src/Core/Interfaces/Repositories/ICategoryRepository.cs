using System.Security.Principal;
using MTS.Core.Abstraction;
using MTS.Core.Models;

namespace MTS.Core.Interfaces.Repositories
{
    public interface ICategoryRepository : IIdentityRepository<string, CategoryModel>
    {

    }
}