using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.RepositoriesInterfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetOneAsync(string id);
        Task<List<Category>> GetAllAsync();
        Task AddOneAsync(Category category);
        Task RemoveOneAsync(string name);
    }
}