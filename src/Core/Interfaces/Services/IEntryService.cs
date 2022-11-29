using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO;
using Core.Entities;
using Core.Models;

namespace Core.Interfaces.Services;

public interface IEntryService
{
    Task AddEntryAsync(EntryDTO entryInput);
    Task<AllCategoryEntries> GetAllEntriesAsync(string categoryId, bool withContent);
    Task<EntryModel> GetEntryAsync(string id);
    Task<List<EntryModel>> GetRecentAsync();
    Task RemoveEntryAsync(string id);
    Task UpdateEntryAsync(EntryDTO entryInput);
    Task UpdateEntryWithoutContentAsync(EntryDTO entryInput);
}