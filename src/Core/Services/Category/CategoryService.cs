using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MTS.Core.DTO;
using MTS.Core.Entities;
using MTS.Core.Interfaces.Repositories;
using MTS.Core.Interfaces.Services;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;

namespace MTS.Core.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly ICurrentUserService _currentUser;
        public CategoryService(ICategoryRepository repository, ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<CategoryModel?> GetCategoryAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null!;

            var category = await _repository.GetAsync(id);

            return category!;
        }

        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            return await _repository.AsIdentityQueryable()
                .OrderByDescending(cat => cat.LastEditedOnDate)
                .ToListAsync();
        }

        public async Task<List<CategoryModel>> GetRootCategoriesAsync()
        {
            if (string.IsNullOrWhiteSpace(_currentUser.UserId))
                throw new HandledException("Owner ID cannot be empty");

            return await _repository.AsIdentityQueryable()
                .Where(cat => cat.Level == 0)
                .ToListAsync();
        }

        public async Task<List<CategoryModel>> GetSubCategoriesAsync(string parentId)
        {
            if (string.IsNullOrWhiteSpace(parentId) && string.IsNullOrWhiteSpace(_currentUser.UserId))
                throw new HandledException("Parent ID and Owner ID cannot be empty");

            return await _repository.AsIdentityQueryable()
                .Where(category => category.ParentCategoryId == parentId)
                .OrderByDescending(cat => cat.LastEditedOnDate)
                .ToListAsync();
        }

        public async Task AddCategoryAsync(CategoryDto categoryInput)
        {
            if (string.IsNullOrWhiteSpace(categoryInput.Name))
                throw new HandledException("Name cannot be empty");

            var exists = await _repository.AsIdentityQueryable().AnyAsync(cat =>
                cat.Name == categoryInput.Name
                && cat.ParentCategoryId == categoryInput.CategoryParentId);

            if (exists)
                throw new HandledException("This category already exists");

            CategoryModel parent;
            string path = _currentUser?.UserId ?? string.Empty;
            int level = 0;

            if (!string.IsNullOrEmpty(categoryInput.CategoryParentId))
            {
                parent = await GetCategoryAsync(categoryInput.CategoryParentId);

                if (parent is null)
                    throw new HandledException("This parent category doesn't exist");

                path = parent.Path;
                level = parent.Level + 1;
            }

            string newCategoryId = Guid.NewGuid().ToString();
            CategoryModel newCategory = new()
            {
                Id = newCategoryId,
                CreatedDate = DateTime.UtcNow,
                LastEditedOnDate = DateTime.UtcNow,
                Name = categoryInput.Name.Trim(),
                ParentCategoryId = categoryInput.CategoryParentId,
                Level = (byte)level,
                Path = $"{path}/{newCategoryId}",
                UserId = _currentUser.UserId,
            };

            await _repository.AddAsync(newCategory);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(CategoryDto categoryInput)
        {
            if (string.IsNullOrWhiteSpace(categoryInput.Name))
                throw new HandledException("Name cannot be empty");

            // check if under parent the same category name already exists
            if (!string.IsNullOrEmpty(categoryInput.CategoryParentId))
            {
                var isNameDuplicate = await _repository.AsIdentityQueryable().AnyAsync(entry =>
                        entry.ParentCategoryId == categoryInput.CategoryParentId
                        && entry.Name == categoryInput.Name);

                if (isNameDuplicate)
                    throw new HandledException("Couldn't update that category, this name is already being used");
            }

            // check if this category exists
            var category = await _repository.AsIdentityQueryable().FirstOrDefaultAsync(cat =>
                cat.Id == categoryInput.CategoryId
                && cat.ParentCategoryId == categoryInput.CategoryParentId);

            if (category is null)
                throw new HandledException("Couldn't update that category");

            category.Name = categoryInput.Name;
            category.LastEditedOnDate = DateTime.UtcNow;

            await _repository.UpdateAsync(category);
            await _repository.SaveChangesAsync();
        }

        public async Task RemoveCategoryAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new HandledException("Name cannot be empty");

            var category = await _repository.GetAsync(id);
            if (category is null)
                throw new HandledException("Couldn't find that category");

            await _repository.RemoveAsync(category);
            await _repository.SaveChangesAsync();
        }

        public async Task<CategoryModel> GetCategoryWithEntriesAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new HandledException("Category ID cannot be empty");

            var categoryWithEntries = await _repository.AsIdentityQueryable()
                .Include(cat => cat.Entries.OrderByDescending(e => e.LastUpdatedOn))
                .FirstOrDefaultAsync(cat => cat.Id == id);

            return categoryWithEntries;
        }
    }
}