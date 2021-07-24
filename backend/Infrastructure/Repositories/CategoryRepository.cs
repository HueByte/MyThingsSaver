using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task AddOneAsync(Category cat, string ownerName)
        {
            if (string.IsNullOrWhiteSpace(cat.Name))
                throw new ArgumentException("Name cannot be empty");

            var exists = await _context.Categories.AnyAsync(category => category.Name == cat.Name);
            if (exists)
                throw new Exception("This category already exists");

            var user = await _userManager.FindByNameAsync(ownerName);

            // assign user if found
            cat.Owner = user ?? throw new Exception("Something went wrong");

            await _context.Categories.AddAsync(cat);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOneAsync(string name, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty");

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == name && x.Owner.Id == ownerId);
            if (category == null)
                throw new Exception("Couldn't find that category");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}