using System.Threading.Tasks;
using Core.Models;

namespace Core.RepositoriesInterfaces
{
    public interface ICategoryRepository
    {
         Task<Category> GetOne(string id);
    }
}