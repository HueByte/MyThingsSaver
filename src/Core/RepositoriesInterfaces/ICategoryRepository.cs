using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO;
using Core.Entities;
using Core.Models;

namespace Core.RepositoriesInterfaces
{
    [Obsolete]
    public interface ICategoryRepository2
    {
        Task<List<CategoryModel>> GetAllAsync();
        Task<List<CategoryModel>> GetRootCategoriesAsync();
        Task<List<CategoryModel>> GetSubcategoriesAsync(string parentId);
        Task<CategoryModel> GetOneByIdAsync(string name);
        Task<CategoryModel> GetCategoryWithEntriesAsync(string categoryId);
        Task AddOneAsync(CategoryDto category);
        Task RemoveOneAsync(string name);
        Task UpdateOneAsync(CategoryDto newCategory);
    }
}