using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Core.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [Obsolete]
    public class CategoryEntryRepository2 : ICategoryEntryRepository2
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public CategoryEntryRepository2(AppDbContext context, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        public async Task<EntryModel> GetOneByIdAsync(Guid id)
        {
            return await GetOneByIdAsync(id.ToString());
        }

        public async Task<EntryModel> GetOneByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new EndpointException("Entry ID cannot be empty");

            var entry = await _context.Entries
                .FirstOrDefaultAsync(entry => entry.Id == id && entry.Owner.Id == _currentUserService.UserId);

            if (entry == null)
                throw new EndpointException("Couldn't find that entry");

            return entry;
        }

        public async Task<AllCategoryEntries> GetAllAsync(string categoryId, bool withContent)
        {
            AllCategoryEntries entries = new();

            if (string.IsNullOrEmpty(_currentUserService.UserId) || string.IsNullOrEmpty(categoryId))
                throw new EndpointException("Owner ID or Category ID was empty");

            if (withContent)
            {
                entries.SubCategories = await _context.Categories
                    .Where(cat => cat.ParentCategoryId == categoryId)
                    .OrderByDescending(e => e.LastEditedOnDate)
                    .ToListAsync();

                entries.Entries = await _context.Entries
                    .Where(entry => entry.CategoryId == categoryId && entry.Owner.Id == _currentUserService.UserId)
                    .OrderByDescending(e => e.LastUpdatedOn)
                    .ToListAsync();
            }
            else
            {
                entries.SubCategories = await _context.Categories
                    .Where(cat => cat.ParentCategoryId == categoryId)
                    .OrderByDescending(e => e.LastEditedOnDate)
                    .ToListAsync();

                entries.Entries = await _context.Entries
                    .Where(entry => entry.CategoryId == categoryId && entry.Owner.Id == _currentUserService.UserId)
                    .Select(e => new EntryModel
                    {
                        CreatedOn = e.CreatedOn,
                        Id = e.Id,
                        Name = e.CategoryEntryName,
                        Image = e.Image,
                        UserId = e.UserId,
                        Size = e.Size,
                        LastUpdatedOn = e.LastUpdatedOn,
                        CategoryId = e.CategoryId
                    })
                    .OrderByDescending(e => e.LastUpdatedOn)
                    .ToListAsync();
            }

            return entries;
        }

        public async Task AddOneAsync(EntryDTO entryDTO)
        {
            if (string.IsNullOrWhiteSpace(entryDTO.EntryName))
                throw new EndpointException("Entry name cannot be empty");

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == entryDTO.CategoryId);
            if (category is null)
                throw new EndpointException("Couldn't find that category");

            category.LastEditedOnDate = DateTime.UtcNow;

            var entry = new EntryModel()
            {
                Name = entryDTO.EntryName,
                CategoryId = entryDTO.CategoryId,
                Content = "",
                Size = ASCIIEncoding.Unicode.GetByteCount(entryDTO.Content),
                CreatedOn = DateTime.UtcNow,
                LastUpdatedOn = DateTime.UtcNow,
                Image = entryDTO.Image,
                UserId = _currentUserService.UserId,
                Id = Guid.NewGuid().ToString()
            };

            await _context.Entries.AddAsync(entry);
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOneAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new EndpointException("ID cannot be empty");

            // make sure that user owns that entry
            var entry = await _context.Entries.FirstOrDefaultAsync(entry => entry.Id == id && entry.UserId == _currentUserService.UserId);
            if (entry == null)
                throw new EndpointException("couldn't find that entry");


            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == entry.CategoryId);
            if (category is null)
                throw new EndpointException("Couldn't find that category");

            category.LastEditedOnDate = DateTime.UtcNow;

            _context.Entries.Remove(entry);
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOneAsync(EntryDTO newEntry)
        {
            if (string.IsNullOrWhiteSpace(newEntry.EntryId))
                throw new EndpointException("ID cannot be empty");

            var entry = await _context.Entries.FirstOrDefaultAsync(entry => entry.Id == newEntry.EntryId
                                                                                      && entry.UserId == _currentUserService.UserId);
            if (entry == null)
                throw new EndpointException("Couldn't find that entry");

            entry.Name = newEntry.EntryName.Trim();
            entry.Content = newEntry.Content;
            entry.Size = ASCIIEncoding.Unicode.GetByteCount(newEntry.Content);
            entry.LastUpdatedOn = DateTime.UtcNow;

            _context.Entries.Update(entry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOneWithoutContentAsync(EntryDTO newEntry)
        {
            if (string.IsNullOrWhiteSpace(newEntry.EntryId))
                throw new EndpointException("ID cannot be empty");

            var entry = await _context.Entries.FirstOrDefaultAsync(entry => entry.Id == newEntry.EntryId
                                                                                      && entry.UserId == _currentUserService.UserId);

            if (entry == null)
                throw new EndpointException("Couldn't find that entry");

            entry.Name = newEntry.EntryName.Trim();
            entry.CategoryId = newEntry.CategoryId;
            entry.LastUpdatedOn = DateTime.UtcNow;

            _context.Entries.Update(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EntryModel>> GetRecentAsync()
        {
            var entries = await _context.Entries
                .Where(entry => entry.UserId == _currentUserService.UserId)
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
}