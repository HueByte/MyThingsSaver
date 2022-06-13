using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO;
using Core.Entities;
using Core.Models;

namespace Core.RepositoriesInterfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync(string ownerId);
        Task<List<Category>> GetRootCategoriesAsync(string ownerId);
        Task<List<Category>> GetSubcategoriesAsync(string parentId, string ownerId);
        Task<Category> GetOneByIdAsync(string name, string ownerId);
        Task<Category> GetCategoryWithEntriesAsync(string categoryId, string ownerId);
        Task AddOneAsync(CategoryDto category, string ownerId);
        Task RemoveOneAsync(string name, string ownerId);
        Task UpdateOneAsync(CategoryDto newCategory, string ownerId);
        Task UpdateMultipleAsync(List<Category> newCategories, string ownerId);
    }
}