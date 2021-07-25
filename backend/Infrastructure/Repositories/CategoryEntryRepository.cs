using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryEntryRepository : ICategoryEntryRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoryEntryRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<CategoryEntry> GetOneAsync(Guid id, string ownerId)
        {
            return await GetOneAsync(id.ToString(), ownerId);
        }

        public async Task<CategoryEntry> GetOneAsync(string id, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be empty");

            var entry = await _context.CategoriesEntries.FirstOrDefaultAsync(ent => ent.CategoryEntryId.ToString() == id && ent.Owner.Id == ownerId);

            if (entry == null)
                throw new Exception("Couldn't find that entry");

            return entry;
        }

        public async Task<List<CategoryEntry>> GetAllAsync(string ownerId)
        {
            var entries = await _context.CategoriesEntries.Where(entry => entry.Owner.Id == ownerId).ToListAsync();
            if (entries.Count == 0 || entries == null)
                throw new Exception("Couldn't find any entries");

            return entries;
        }

        public async Task AddOneAsync(CategoryEntry entry, string ownerName)
        {
            if (entry == null)
                throw new ArgumentException("Entry cannot be null");

            var user = await _userManager.FindByNameAsync(ownerName);
            entry.Owner = user ?? throw new Exception("User not found");

            await _context.CategoriesEntries.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOneAsync(string id, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be empty");

            // make sure that user owns that entry
            var entry = await _context.CategoriesEntries.FirstOrDefaultAsync(entry => entry.CategoryEntryId.ToString() == id && entry.Owner.Id == ownerId);
            if (entry == null)
                throw new Exception("couldn't find that entry");

            _context.CategoriesEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}