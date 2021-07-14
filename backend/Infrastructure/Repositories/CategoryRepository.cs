using System.Threading.Tasks;
using Core.RepositoriesInterfaces;
using Core.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetOne(string id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.CategoryId.ToString() == id);
            
            if(category != null)
                return category;
            
            return null;
        }

        public async Task<List<Category>> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();
            if(categories.Count != 0 && categories != null)
                return categories;

            return null;
        }

        public async Task AddOne(Category cat)
        {
            await _context.Categories.AddAsync(cat);
            await _context.SaveChangesAsync();
        }
    }
}