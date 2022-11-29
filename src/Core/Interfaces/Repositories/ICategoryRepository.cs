using System.Security.Principal;
using Core.Abstraction;
using Core.Models;

namespace Core.Interfaces.Repositories
{
    public interface ICategoryRepository : IIdentityRepository<string, CategoryModel>
    {

    }
}