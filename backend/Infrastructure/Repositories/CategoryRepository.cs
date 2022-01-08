using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTO;
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

        public async Task<List<Category>> GetRootCategoriesAsync(string ownerId)
        {
            if (string.IsNullOrWhiteSpace(ownerId))
                throw new ArgumentException("Owner ID cannot be empty");

            var rootCategories = await _context.Categories.Where(category => category.Level == 0).ToListAsync();

            return rootCategories;
        }

        public async Task<List<Category>> GetSubcategoriesAsync(string parentId, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(parentId) && string.IsNullOrWhiteSpace(ownerId))
                throw new ArgumentException("Parent ID and Owner ID cannot be empty");

            var subCategories = await _context.Categories.Where(category => category.ParentCategoryId == parentId
                                                                      && category.OwnerId == ownerId).ToListAsync();

            return subCategories;
        }

        public async Task AddOneAsync(CategoryDTO cat, string ownerId)
        {
            if (string.IsNullOrWhiteSpace(cat.Name))
                throw new ArgumentException("Name cannot be empty");


            var exists = await _context.Categories.AnyAsync(category => category.Name == cat.Name
                                                                && category.OwnerId == ownerId
                                                                && category.ParentCategoryId == cat.CategoryParentId);
            if (exists)
                throw new Exception("This category already exists");

            Category parent;
            string path = ownerId;
            byte level = 0;
            if (!string.IsNullOrWhiteSpace(cat.CategoryParentId))
            {
                parent = await _context.Categories.FirstOrDefaultAsync(category => category.OwnerId == ownerId
                                                                            && category.CategoryId == cat.CategoryParentId);

                if (parent is null)
                    throw new NullReferenceException("Parent category doesn't exist");

                path = parent.Path;
                level = (byte)(parent.Level + 1);
            }

            string newCategoryId = Guid.NewGuid().ToString();
            Category newCategory = new()
            {
                CategoryEntries = null,
                CategoryId = newCategoryId,
                DateCreated = DateTime.UtcNow,
                Name = cat.Name.Trim(),
                LastEditedOn = DateTime.UtcNow,
                OwnerId = ownerId,
                ParentCategoryId = cat.CategoryParentId,
                Path = $"{path}/{newCategoryId}",
                Level = level
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

        public async Task UpdateMultipleAsync(List<Category> newCategories, string ownderId)
        {
            _context.Categories.UpdateRange(newCategories);
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

        private string[] ParsePath(string path) => path.Split('/');
    }
}