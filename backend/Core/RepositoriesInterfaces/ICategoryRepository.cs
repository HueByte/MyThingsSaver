using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Models;

namespace Core.RepositoriesInterfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetOneAsync(string name, string ownerId);
        Task<List<Category>> GetAllAsync(string ownerId);
        Task AddOneAsync(CategoryDTO category, string ownerId);
        Task RemoveOneAsync(string name, string ownerId);
        Task UpdateOneAsync(CategoryDTO newCategory, string ownerId);
        Task<Category> GetCategoryWithEntriesAsync(string categoryId, string ownerId);
    }
}