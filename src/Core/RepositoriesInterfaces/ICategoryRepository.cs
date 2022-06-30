using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO;
using Core.Entities;
using Core.Models;

namespace Core.RepositoriesInterfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<List<Category>> GetRootCategoriesAsync();
        Task<List<Category>> GetSubcategoriesAsync(string parentId);
        Task<Category> GetOneByIdAsync(string name);
        Task<Category> GetCategoryWithEntriesAsync(string categoryId);
        Task AddOneAsync(CategoryDto category);
        Task RemoveOneAsync(string name);
        Task UpdateOneAsync(CategoryDto newCategory);
    }
}