using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Models;

namespace Core.RepositoriesInterfaces
{
    public interface ICategoryEntryRepository
    {
        Task<CategoryEntry> GetOneByIdAsync(Guid id, string ownerId);
        Task<CategoryEntry> GetOneByIdAsync(string id, string ownerId);
        Task<CategoryEntry> GetOneByNameAsync(string categoryId, string name, string ownerId);
        Task<List<CategoryEntry>> GetAllAsync(string categoryId, string ownerId);
        Task AddOneAsync(CategoryEntryDTO entity, string ownerId);
        Task RemoveOneAsync(string categoryId, string id, string ownerId);
    }
}