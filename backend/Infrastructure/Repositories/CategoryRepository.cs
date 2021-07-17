using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetOneAsync(string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.name == name);

            if (category == null)
                throw new Exception("Couldn't find this category");

            return category;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories.Count != 0 && categories != null)
                return categories;

            return null;
        }

        public async Task AddOneAsync(Category cat)
        {
            if (string.IsNullOrEmpty(cat.name))
                throw new Exception("Name cannot be empty");

            await _context.Categories.AddAsync(cat);
            await _context.SaveChangesAsync();
        }
    }
}