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
        Task<CategoryEntry> GetOneByIdAsync(Guid id, string ownerId);
        Task<CategoryEntry> GetOneByIdAsync(string id, string ownerId);
        Task<AllCategoryEntries> GetAllAsync(string categoryId, string ownerId, bool withContent = false);
        Task<List<CategoryEntry>> GetRecentAsync(string ownerId);
        Task AddOneAsync(CategoryEntryDto entity, string ownerId);
        Task RemoveOneAsync(string id, string ownerId);
        Task UpdateOneAsync(CategoryEntryDto newEntry, string ownerId);
        Task UpdateOneWithoutContentAsync(CategoryEntryDto newEntry, string ownerId);
    }
}