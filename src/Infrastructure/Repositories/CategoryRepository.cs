using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTO;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Core.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public CategoryRepository(AppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Category> GetOneByIdAsync(string id)
        {
            if (id is null)
                throw new EndpointException("ID cannot be empty");

            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.CategoryId == id && cat.Owner.Id == _currentUserService.UserId);

            if (category is null)
                throw new EndpointException("Couldn't find this category");

            return category;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var categories = await _context.Categories
                .Where(cat => cat.Owner.Id == _currentUserService.UserId)
                .OrderByDescending(cat => cat.LastEditedOn)
                .ToListAsync();

            return categories;
        }

        public async Task<List<Category>> GetRootCategoriesAsync()
        {
            if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
                throw new EndpointException("Owner ID cannot be empty");

            var rootCategories = await _context.Categories.Where(category => category.Level == 0 && category.OwnerId == _currentUserService.UserId)
                                                          .ToListAsync();

            return rootCategories;
        }

        public async Task<List<Category>> GetSubcategoriesAsync(string parentId)
        {
            if (string.IsNullOrWhiteSpace(parentId) && string.IsNullOrWhiteSpace(_currentUserService.UserId))
                throw new EndpointException("Parent ID and Owner ID cannot be empty");

            var subCategories = await _context.Categories.Where(category => category.ParentCategoryId == parentId
                                                                      && category.OwnerId == _currentUserService.UserId).ToListAsync();

            return subCategories;
        }

        public async Task AddOneAsync(CategoryDto cat)
        {
            if (string.IsNullOrWhiteSpace(cat.Name))
                throw new EndpointException("Name cannot be empty");


            var exists = await _context.Categories.AnyAsync(category => category.Name == cat.Name
                                                                && category.OwnerId == _currentUserService.UserId
                                                                && category.ParentCategoryId == cat.CategoryParentId);
            if (exists)
                throw new EndpointException("This category already exists");

            Category parent;
            string path = _currentUserService.UserId;
            byte level = 0;
            if (!string.IsNullOrWhiteSpace(cat.CategoryParentId))
            {
                parent = await _context.Categories.FirstOrDefaultAsync(category => category.OwnerId == _currentUserService.UserId
                                                                            && category.CategoryId == cat.CategoryParentId);

                if (parent is null)
                    throw new EndpointException("This parent category doesn't exist");

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
                OwnerId = _currentUserService.UserId,
                ParentCategoryId = cat.CategoryParentId,
                Path = $"{path}/{newCategoryId}",
                Level = level
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOneAsync(CategoryDto newCategory)
        {
            if (string.IsNullOrWhiteSpace(newCategory.Name))
                throw new EndpointException("Name cannot be empty");

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == newCategory.CategoryId && x.Owner.Id == _currentUserService.UserId);
            if (category == null)
                throw new EndpointException("Couldn't find that category");

            category.Name = newCategory.Name.Trim();
            category.LastEditedOn = DateTime.UtcNow;

            var doesExist = _context.Categories.Any(x => x.Name == category.Name
                                                         && x.ParentCategoryId == category.ParentCategoryId
                                                         && x.OwnerId == category.OwnerId);
            if (doesExist)
                throw new EndpointException("There's already category with that name");

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveOneAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new EndpointException("Name cannot be empty");

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id && x.Owner.Id == _currentUserService.UserId);
            if (category == null)
                throw new EndpointException("Couldn't find that category");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetCategoryWithEntriesAsync(string categoryId)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                throw new EndpointException("Category ID cannot be empty");

            var categoryWithEntries = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<Category>(_context.Categories
                    .Include(entity => entity.CategoryEntries.OrderByDescending(e => e.LastUpdatedOn)),
                param => param.CategoryId == categoryId && param.OwnerId == _currentUserService.UserId);

            return categoryWithEntries;
        }

        private static string[] ParsePath(string path) => path.Split('/');
    }
}