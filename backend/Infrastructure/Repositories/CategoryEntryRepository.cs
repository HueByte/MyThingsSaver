using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Models;
using Core.RepositoriesInterfaces;
using Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryEntryRepository : BaseRepository<CategoryEntry>, ICategoryEntryRepository
    {
        private readonly AppDbContext _context;
        public CategoryEntryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryEntry> GetOneAsync(Guid id)
        {
            return await GetOneAsync(id.ToString());
        }

        public override async Task<CategoryEntry> GetOneAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be empty");

            var entry = await _context.CategoriesEntries.FirstOrDefaultAsync(ent => ent.EntryId.ToString() == id);

            if (entry == null)
                throw new Exception("Couldn't find that entry");

            return entry;
        }

        public override async Task<List<CategoryEntry>> GetAllAsync()
        {
            var entries = await _context.CategoriesEntries.ToListAsync();
            if (entries.Count == 0 || entries == null)
                throw new Exception("Couldn't find any entries");

            return entries;
        }

        public override async Task AddOneAsync(CategoryEntry entry)
        {
            if (entry == null)
                throw new ArgumentException("Entry cannot be null");

            await _context.CategoriesEntries.AddAsync(entry);
            await _context.SaveChangesAsync();
        }

        public override async Task RemoveOneAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be empty");

            var entry = await _context.CategoriesEntries.FirstOrDefaultAsync(x => x.EntryId.ToString() == id);
            if (entry == null)
                throw new Exception("couldn't find that entry");

            _context.CategoriesEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}