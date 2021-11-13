using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetOneByIdAsync(string id, string ownerId)
        {
            if (id is null)
                throw new Exception("ID cannot be empty");

            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.CategoryId == id && cat.Owner.Id == ownerId);

            if (category is null)
                throw new Exception("Couldn't find this category");

            return category;
        }

        public async Task<List<Category>> GetAllAsync(string ownerId)
        {
            var categories = await _context.Categories
                .Where(cat => cat.Owner.Id == ownerId)
                .OrderByDescending(cat => cat.LastEditedOn)
                .ToListAsync();

            return categories;
        }

        public async Task AddOneAsync(CategoryDTO cat, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(cat.Name))
                throw new ArgumentException("Name cannot be empty");

            var exists = await _context.Categories.AnyAsync(category => category.Name == cat.Name
                                                                && category.OwnerId == ownerId
                                                                && cat.CategoryParentId == category.ParentCategoryId);
            if (exists)
                throw new Exception("This category already exists");

            Category parentCategory;
            if (!string.IsNullOrEmpty(cat.CategoryParentId))
            {
                parentCategory = await _context.Categories.FirstOrDefaultAsync(category => category.CategoryId == cat.CategoryParentId && category.OwnerId == ownerId);
                if (parentCategory is not null && !string.IsNullOrEmpty(parentCategory.ParentCategoryId))
                    throw new Exception("This category cannot have more subcategories");
            }


            Category newCategory = new()
            {
                CategoryEntries = null,
                CategoryId = Guid.NewGuid().ToString(),
                DateCreated = DateTime.UtcNow,
                Name = cat.Name.Trim(),
                LastEditedOn = DateTime.UtcNow,
                OwnerId = ownerId,
                ParentCategoryId = cat.CategoryParentId
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

            category.Name = newCategory.Name.Trim();
            category.LastEditedOn = DateTime.UtcNow;

            var doesExist = _context.Categories.Any(x => x.Name == category.Name);
            if (doesExist)
                throw new Exception("There's already category with that name");

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

            var subCategories = await _context.Categories.Where(x => x.ParentCategoryId == id).ToListAsync();
            if (subCategories is not null)
                _context.Categories.RemoveRange(subCategories);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetCategoryWithEntriesAsync(string categoryId, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                throw new ArgumentException("Category ID cannot be empty");

            var categoryWithEntries = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Category>(_context.Categories
                    .Include(entity => entity.CategoryEntries.OrderByDescending(e => e.LastUpdatedOn)),
                param => param.CategoryId == categoryId && param.OwnerId == ownerId);

            return categoryWithEntries;
        }
    }
}