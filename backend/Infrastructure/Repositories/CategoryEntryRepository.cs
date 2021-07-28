using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Identity;
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

        public async Task<CategoryEntry> GetOneByIdAsync(string categoryId, Guid id, string ownerId)
        {
            return await GetOneByIdAsync(categoryId, id.ToString(), ownerId);
        }

        public async Task<CategoryEntry> GetOneByIdAsync(string categoryId, string id, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(categoryId))
                throw new ArgumentException("Category ID and Entry ID cannot be empty");

            var entry = await _context.CategoriesEntries.FirstOrDefaultAsync(entry => entry.CategoryId == categoryId && entry.CategoryEntryId.ToString() == id && entry.Owner.Id == ownerId);

            if (entry == null)
                throw new Exception("Couldn't find that entry");

            return entry;
        }

        public async Task<CategoryEntry> GetOneByNameAsync(string categoryId, string name, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(categoryId) || string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category ID and Entry name cannot be empty");

            var entry = await _context.CategoriesEntries.FirstOrDefaultAsync(entry => entry.CategoryId == categoryId && entry.CategoryEntryName == name && entry.OwnerId == ownerId);

            if (entry == null)
                throw new Exception("Couldn't find that entry");

            return entry;
        }

        public async Task<List<CategoryEntry>> GetAllAsync(string categoryId, string ownerId)
        {
            var entries = await _context.CategoriesEntries.Where(entry => entry.CategoryId == categoryId && entry.Owner.Id == ownerId).ToListAsync();
            if (entries.Count == 0 || entries == null)
                throw new Exception("Couldn't find any entries");

            return entries;
        }

        public async Task AddOneAsync(CategoryEntryDTO entryDTO, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(entryDTO.CategoryName))
                throw new ArgumentException("Category name cannot be empty, something went wrong");

            var entry = new CategoryEntry()
            {
                CategoryEntryName = entryDTO.EntryName,
                CategoryId = entryDTO.CategoryId,
                Content = entryDTO.Content,
                CreatedOn = DateTime.UtcNow,
                image = entryDTO.Image,
                OwnerId = ownerId,
                CategoryEntryId = Guid.NewGuid().ToString()
            };

            await _context.CategoriesEntries.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOneAsync(string categoryId, string id, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be empty");

            // make sure that user owns that entry
            var entry = await _context.CategoriesEntries.FirstOrDefaultAsync(entry => entry.CategoryId == categoryId && entry.CategoryEntryId.ToString() == id && entry.Owner.Id == ownerId);
            if (entry == null)
                throw new Exception("couldn't find that entry");

            _context.CategoriesEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}