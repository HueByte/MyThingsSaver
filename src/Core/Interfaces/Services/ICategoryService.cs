using System.Collections.Generic;
using System.Threading.Tasks;
using MTS.Core.DTO;
using MTS.Core.Models;

namespace MTS.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryDto categoryInput);
        Task<List<CategoryModel>> GetAllCategoriesAsync();
        Task<CategoryModel> GetCategoryAsync(string id);
        Task<CategoryModel> GetCategoryWithEntriesAsync(string id);
        Task<List<CategoryModel>> GetRootCategoriesAsync();
        Task<List<CategoryModel>> GetSubCategoriesAsync(string parentId);
        Task RemoveCategoryAsync(string id);
        Task UpdateCategoryAsync(CategoryDto categoryInput);
    }
}