using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public override async Task<Category> GetOneAsync(string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.name == name);

            if (category == null)
                throw new Exception("Couldn't find this category");

            return category;
        }

        public override async Task<List<Category>> GetAllAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories.Count == 0 || categories == null)
                throw new Exception("Couldn't find any categories");

            return categories;
        }

        public override async Task AddOneAsync(Category cat)
        {
            if (string.IsNullOrWhiteSpace(cat.name))
                throw new ArgumentException("Name cannot be empty");

            await _context.Categories.AddAsync(cat);
            await _context.SaveChangesAsync();
        }

        public override async Task RemoveOneAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty");

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.name == name);
            if (category == null)
                throw new Exception("Couldn't find that category");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}