using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Entry;

public class EntryService
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

    public async Task<EntryModel> GetEntryAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        return await _repository.GetAsync(id);
    }

    public async Task<AllCategoryEntries> GetAllEntriesAsync(string categoryId, bool withContent)
    {
        AllCategoryEntries entries = new();

        if (string.IsNullOrEmpty(_currentUser.UserId) || string.IsNullOrEmpty(categoryId))
            return null;

        entries.SubCategories = await _categoryService.GetSubCategoriesAsync(categoryId);

        var entriesQuery = await _repository.GetAllAsync();
        entriesQuery.Where(entry => entry.CategoryId == categoryId);

        if (withContent)
        {
            entriesQuery.Select(entry => new EntryModel
            {
                CreatedOn = entry.CreatedOn,
                Id = entry.Id,
                Name = entry.Name,
                Image = entry.Image,
                UserId = entry.UserId,
                Size = entry.Size,
                LastUpdatedOn = entry.LastUpdatedOn,
                CategoryId = entry.CategoryId
            });
        }

        entries.Entries = await entriesQuery
            .OrderByDescending(entry => entry.LastUpdatedOn)
            .ToListAsync();

        return entries;
    }

    public async Task AddEntryAsync(CategoryEntryDto entryInput)
    {
        if (string.IsNullOrWhiteSpace(entryInput.EntryName))
            throw new EndpointException("Entry name cannot be empty");

        var category = await _categoryService.GetCategoryAsync(entryInput.CategoryId);
        if (category is null)
            throw new EndpointException("Couldn't find that category");

        category.LastEditedOnDate = DateTime.UtcNow;

        var entry = new EntryModel()
        {
            Name = entryInput.EntryName,
            CategoryId = entryInput.CategoryId,
            Content = "",
            Size = ASCIIEncoding.Unicode.GetByteCount(entryInput.Content),
            CreatedOn = DateTime.UtcNow,
            LastUpdatedOn = DateTime.UtcNow,
            Image = entryInput.Image,
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
            throw new EndpointException("ID cannot be empty");

        var entry = await _repository.GetAsync(id);
        if (entry is null)
            throw new EndpointException("Couldn't find that entry");

        var category = await _categoryService.GetCategoryAsync(entry.CategoryId);
        if (category is null)
            throw new EndpointException("Couldn't find that category");

        category.LastEditedOnDate = DateTime.UtcNow;

        await _repository.RemoveAsync(entry);
        await _categoryRepository.UpdateAsync(category);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateEntryAsync(CategoryEntryDto entryInput)
    {
        if (string.IsNullOrWhiteSpace(entryInput.EntryId))
            throw new EndpointException("ID cannot be empty");

        var entry = await _repository.GetAsync(entryInput.EntryId);
        if (entry is null)
            throw new EndpointException("Couldn't find that entry");

        entry.Name = entryInput.EntryName.Trim();
        entry.Content = entryInput.Content;
        entry.Size = ASCIIEncoding.Unicode.GetByteCount(entryInput.Content);
        entry.LastUpdatedOn = DateTime.UtcNow;

        await _repository.UpdateAsync(entry);
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateEntryWithoutContentAsync(CategoryEntryDto entryInput)
    {
        if (string.IsNullOrWhiteSpace(entryInput.EntryId))
            throw new EndpointException("ID cannot be empty");

        var entry = await _repository.GetAsync(entryInput.EntryId);
        if (entry is null)
            throw new EndpointException("Couldn't find that entry");

        entry.Name = entryInput.EntryName.Trim();
        entry.CategoryId = entryInput.CategoryId;
        entry.LastUpdatedOn = DateTime.UtcNow;

        await _repository.UpdateAsync(entry);
        await _repository.SaveChangesAsync();
    }

    public async Task<List<EntryModel>> GetRecentAsync()
    {
        var entriesQuery = await _repository.GetAllAsync();

        var entries = await entriesQuery
            .Include(x => x.Category)
            .Select(x => new EntryModel
            {
                Name = x.Name,
                Id = x.Id,
                Size = x.Size,
                CreatedOn = x.CreatedOn,
                LastUpdatedOn = x.LastUpdatedOn,
                Category = x.Category
            })
            .OrderByDescending(entry => entry.LastUpdatedOn)
            .Take(15)
            .ToListAsync();

        return entries;
    }
}