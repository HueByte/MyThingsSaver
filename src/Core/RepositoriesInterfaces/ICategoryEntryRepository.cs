using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO;
using Core.Entities;
using Core.Models;

namespace Core.RepositoriesInterfaces
{
    public interface ICategoryEntryRepository
    {
        Task<CategoryEntry> GetOneByIdAsync(Guid id);
        Task<CategoryEntry> GetOneByIdAsync(string id);
        Task<AllCategoryEntries> GetAllAsync(string categoryId, bool withContent = false);
        Task<List<CategoryEntry>> GetRecentAsync();
        Task AddOneAsync(CategoryEntryDto entity);
        Task RemoveOneAsync(string id);
        Task UpdateOneAsync(CategoryEntryDto newEntry);
        Task UpdateOneWithoutContentAsync(CategoryEntryDto newEntry);
    }
}