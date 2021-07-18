using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Core.RepositoriesInterfaces
{
    public interface ICategoryEntryRepository
    {
        Task<CategoryEntry> GetOneAsync(Guid id);
        Task<CategoryEntry> GetOneAsync(string id);
        Task<List<CategoryEntry>> GetAllAsync();
        Task AddOneAsync(CategoryEntry entity);
        Task RemoveOneAsync(string id);
    }
}