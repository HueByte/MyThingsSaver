using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Events;
using Core.DTO;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryEntryRepository : ICategoryEntryRepository
    {
        private readonly AppDbContext _context;

        public CategoryEntryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryEntry> GetOneByIdAsync(Guid id, string ownerId)
        {
            return await GetOneByIdAsync(id.ToString(), ownerId);
        }

        public async Task<CategoryEntry> GetOneByIdAsync(string id, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new EndpointException("Entry ID cannot be empty");

            var entry = await _context.CategoriesEntries
                .FirstOrDefaultAsync(entry => entry.CategoryEntryId == id && entry.Owner.Id == ownerId);

            if (entry == null)
                throw new EndpointException("Couldn't find that entry");

            return entry;
        }

        public async Task<AllCategoryEntries> GetAllAsync(string categoryId, string ownerId, bool withContent)
        {
            AllCategoryEntries entries = new();

            if (string.IsNullOrEmpty(ownerId) || string.IsNullOrEmpty(categoryId))
                throw new EndpointException("Owner ID or Category ID was empty");

            if (withContent)
            {
                entries.SubCategories = await _context.Categories
                    .Where(cat => cat.ParentCategoryId == categoryId)
                    .OrderByDescending(e => e.LastEditedOn)
                    .ToListAsync();

                entries.CategoryEntries = await _context.CategoriesEntries
                    .Where(entry => entry.CategoryId == categoryId && entry.Owner.Id == ownerId)
                    .OrderByDescending(e => e.LastUpdatedOn)
                    .ToListAsync();
            }
            else
            {
                entries.SubCategories = await _context.Categories
                    .Where(cat => cat.ParentCategoryId == categoryId)
                    .OrderByDescending(e => e.LastEditedOn)
                    .ToListAsync();

                entries.CategoryEntries = await _context.CategoriesEntries
                    .Where(entry => entry.CategoryId == categoryId && entry.Owner.Id == ownerId)
                    .Select(e => new CategoryEntry
                    {
                        CreatedOn = e.CreatedOn,
                        CategoryEntryId = e.CategoryEntryId,
                        CategoryEntryName = e.CategoryEntryName,
                        Image = e.Image,
                        OwnerId = e.OwnerId,
                        Size = e.Size,
                        LastUpdatedOn = e.LastUpdatedOn,
                        CategoryId = e.CategoryId
                    })
                    .OrderByDescending(e => e.LastUpdatedOn)
                    .ToListAsync();
            }

            return entries;
        }

        public async Task AddOneAsync(CategoryEntryDto entryDTO, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(entryDTO.EntryName))
                throw new EndpointException("Entry name cannot be empty");

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == entryDTO.CategoryId);
            if (category is null)
                throw new EndpointException("Couldn't find that category");

            category.LastEditedOn = DateTime.UtcNow;

            var entry = new CategoryEntry()
            {
                CategoryEntryName = entryDTO.EntryName,
                CategoryId = entryDTO.CategoryId,
                Content = "",
                Size = ASCIIEncoding.Unicode.GetByteCount(entryDTO.Content),
                CreatedOn = DateTime.UtcNow,
                LastUpdatedOn = DateTime.UtcNow,
                Image = entryDTO.Image,
                OwnerId = ownerId,
                CategoryEntryId = Guid.NewGuid().ToString()
            };

            await _context.CategoriesEntries.AddAsync(entry);
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOneAsync(string id, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new EndpointException("ID cannot be empty");

            // make sure that user owns that entry
            var entry = await _context.CategoriesEntries.FirstOrDefaultAsync(entry => entry.CategoryEntryId == id && entry.OwnerId == ownerId);
            if (entry == null)
                throw new EndpointException("couldn't find that entry");


            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == entry.CategoryId);
            if (category is null)
                throw new EndpointException("Couldn't find that category");

            category.LastEditedOn = DateTime.UtcNow;

            _context.CategoriesEntries.Remove(entry);
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOneAsync(CategoryEntryDto newEntry, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(newEntry.EntryId))
                throw new EndpointException("ID cannot be empty");

            var entry = await _context.CategoriesEntries.FirstOrDefaultAsync(entry => entry.CategoryEntryId == newEntry.EntryId
                                                                                      && entry.OwnerId == ownerId);
            if (entry == null)
                throw new EndpointException("Couldn't find that entry");

            entry.CategoryEntryName = newEntry.EntryName.Trim();
            entry.Content = newEntry.Content;
            entry.Size = ASCIIEncoding.Unicode.GetByteCount(newEntry.Content);
            entry.LastUpdatedOn = DateTime.UtcNow;

            _context.CategoriesEntries.Update(entry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOneWithoutContentAsync(CategoryEntryDto newEntry, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(newEntry.EntryId))
                throw new EndpointException("ID cannot be empty");

            var entry = await _context.CategoriesEntries.FirstOrDefaultAsync(entry => entry.CategoryEntryId == newEntry.EntryId
                                                                                      && entry.OwnerId == ownerId);

            if (entry == null)
                throw new EndpointException("Couldn't find that entry");

            entry.CategoryEntryName = newEntry.EntryName.Trim();
            entry.CategoryId = newEntry.CategoryId;
            entry.LastUpdatedOn = DateTime.UtcNow;

            _context.CategoriesEntries.Update(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryEntry>> GetRecentAsync(string ownerId)
        {
            var entries = await _context.CategoriesEntries
                .Where(entry => entry.OwnerId == ownerId)
                .Include(x => x.Category)
                .Select(x => new CategoryEntry
                {
                    CategoryEntryName = x.CategoryEntryName,
                    CategoryEntryId = x.CategoryEntryId,
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
}