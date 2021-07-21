using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.RepositoriesInterfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetOneAsync(string name, string ownerId);
        Task<List<Category>> GetAllAsync(string ownerId);
        Task AddOneAsync(Category category, string ownerName);
        Task RemoveOneAsync(string name, string ownerId);
    }
}