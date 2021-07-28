using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoryRepository(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Category> GetOneAsync(string name, string ownerId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.Name == name && cat.Owner.Id == ownerId);

            if (category == null)
                throw new Exception("Couldn't find this category");

            return category;
        }

        public async Task<List<Category>> GetAllAsync(string ownerId)
        {
            var categories = await _context.Categories.Where(cat => cat.Owner.Id == ownerId).ToListAsync();
            if (categories.Count == 0 || categories == null)
                throw new Exception("Couldn't find any categories");

            return categories;
        }

        public async Task AddOneAsync(CategoryDTO cat, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(cat.Name))
                throw new ArgumentException("Name cannot be empty");

            var exists = await _context.Categories.AnyAsync(category => category.Name == cat.Name);
            if (exists)
                throw new Exception("This category already exists");


            Category newCategory = new()
            {
                CategoryEntries = null,
                CategoryId = Guid.NewGuid().ToString(),
                DateCreated = DateTime.UtcNow,
                Name = cat.Name,
                OwnerId = ownerId
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOneAsync(CategoryDTO newCategory, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(newCategory.Name))
                throw new ArgumentException("Name cannot be empty");
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == newCategory.CategoryId && x.Owner.Id == ownerId);
            if (category == null)
                throw new Exception("Couldn't find that category");

            category.Name = newCategory.Name;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOneAsync(string id, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Name cannot be empty");

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id && x.Owner.Id == ownerId);
            if (category == null)
                throw new Exception("Couldn't find that category");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}