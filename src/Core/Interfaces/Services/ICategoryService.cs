using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO;
using Core.Models;

namespace Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryDto categoryInput);
        Task<List<CategoryModel>> GetAllCategoriesAsync();
        Task<CategoryModel> GetCategoryAsync(string id);
        Task<CategoryModel> GetCategoryWithEntries(string id);
        Task<List<CategoryModel>> GetRootCategoriesAsync();
        Task<List<CategoryModel>> GetSubCategoriesAsync(string parentId);
        Task RemoveCategoryAsync(string id);
        Task UpdateCategoryAsync(CategoryDto categoryInput);
    }
}