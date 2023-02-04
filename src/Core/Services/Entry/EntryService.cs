using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MTS.Core.DTO;
using MTS.Core.Entities;
using MTS.Core.Interfaces.Repositories;
using MTS.Core.Interfaces.Services;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;

namespace MTS.Core.Services.Entry;



public class EntryService : IEntryService
{
    private readonly ICategoryService _categoryService;
    private readonly IEntryRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUser;
    public EntryService(ICategoryService categoryService, ICategoryRepository categoryRepository, IEntryRepository repository, ICurrentUserService currentUser)
    {
        _categoryService = categoryService;
        _categoryRepository = categoryRepository;
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<EntryModel?> GetEntryAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null!;

        return await _repository
            .AsIdentityQueryable()
            .Include(e => e.PublicEntry)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<AllCategoryEntries?> GetAllEntriesAsync(string categoryId, bool withContent)
    {
        AllCategoryEntries entries = new();

        if (string.IsNullOrEmpty(_currentUser.UserId) || string.IsNullOrEmpty(categoryId))
            return null!;

        entries.SubCategories = await _categoryService.GetSubCategoriesAsync(categoryId);

        var entriesQuery = _repository
            .AsIdentityQueryable()
            .Where(entry => entry.CategoryId == categoryId);

        if (!withContent)
        {
            entriesQuery = entriesQuery.Select(entry => new EntryModel
            {
                CreatedOn = entry.CreatedOn,
                Id = entry.Id,
                Name = entry.Name,
                UserId = entry.UserId,
                Size = entry.Size,
                LastUpdatedOn = entry.LastUpdatedOn,
                CategoryId = entry.CategoryId,
                PublicEntryId = entry.PublicEntryId,
            });
        }

        entries.Entries = await entriesQuery
            .OrderByDescending(entry => entry.LastUpdatedOn)
            .ToListAsync();

        return entries;
    }

    public async Task AddEntryAsync(EntryDTO entryInput)
    {
        if (string.IsNullOrWhiteSpace(entryInput.EntryName))
            throw new HandledException("Entry name cannot be empty");

        var category = await _categoryService.GetCategoryAsync(entryInput.CategoryId);
        if (category is null)
            throw new HandledException("Couldn't find that category");

        category.LastEditedOnDate = DateTime.UtcNow;

        var entry = new EntryModel()
        {
            Name = entryInput.EntryName,
            CategoryId = entryInput.CategoryId,
            Content = "",
            Size = default,
            CreatedOn = DateTime.UtcNow,
            LastUpdatedOn = DateTime.UtcNow,
            UserId = _currentUser.UserId,
            Id = Guid.NewGuid().ToString()
        };

        await _repository.AddAsync(entry);
        await _categoryRepository.UpdateAsync(category);
        await _repository.SaveChangesAsync();
    }

    public async Task RemoveEntryAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new HandledException("ID cannot be empty");

        var entry = await _repository.GetAsync(id);
        if (entry is null)
            throw new HandledException("Couldn't find that entry");

        var category = await _categoryService.GetCategoryAsync(entry.CategoryId);
        if (category is null)
            throw new HandledException("Couldn't find that category");

        category.LastEditedOnDate = DateTime.UtcNow;

        await _repository.RemoveAsync(entry);
        await _categoryRepository.UpdateAsync(category);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateEntryAsync(EntryDTO entryInput)
    {
        if (entryInput is null)
            throw new HandledException("Entry cannot be null");

        if (string.IsNullOrWhiteSpace(entryInput.EntryId))
            throw new HandledException("ID cannot be empty");

        var entry = await _repository.GetAsync(entryInput.EntryId);
        if (entry is null)
            throw new HandledException("Couldn't find that entry");

        entry.Name = entryInput.EntryName?.Trim()!;
        entry.Content = entryInput.Content;
        entry.Size = Encoding.UTF8.GetByteCount(entryInput.Content ?? "");
        entry.LastUpdatedOn = DateTime.UtcNow;

        await _repository.UpdateAsync(entry);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateEntryWithoutContentAsync(EntryDTO entryInput)
    {
        if (entryInput is null)
            throw new HandledException("Entry cannot be null");

        if (string.IsNullOrWhiteSpace(entryInput.EntryId))
            throw new HandledException("ID cannot be empty");

        var entry = await _repository.GetAsync(entryInput.EntryId);
        if (entry is null)
            throw new HandledException("Couldn't find that entry");

        entry.Name = entryInput.EntryName?.Trim()!;
        entry.CategoryId = entryInput.CategoryId!;
        entry.LastUpdatedOn = DateTime.UtcNow;

        await _repository.UpdateAsync(entry);
        await _repository.SaveChangesAsync();
    }

    public async Task<List<EntryModel>> GetRecentAsync()
    {
        var entries = await _repository.AsIdentityQueryable()
            .Include(x => x.Category)
            .Select(x => new EntryModel
            {
                Name = x.Name,
                Id = x.Id,
                Size = x.Size,
                CreatedOn = x.CreatedOn,
                LastUpdatedOn = x.LastUpdatedOn,
                Category = x.Category,
                PublicEntryId = x.PublicEntryId
            })
            .OrderByDescending(entry => entry.LastUpdatedOn)
            .Take(15)
            .ToListAsync();

        return entries;
    }
}