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
            var categoriesQuery = await _repository.GetAllAsync();
            return await categoriesQuery
                .OrderByDescending(cat => cat.LastEditedOnDate)
                .ToListAsync();
        }

        public async Task<List<CategoryModel>> GetRootCategoriesAsync()
        {
            if (string.IsNullOrWhiteSpace(_currentUser.UserId))
                throw new EndpointException("Owner ID cannot be empty");

            var categoriesQuery = await _repository.GetAllAsync();
            return await categoriesQuery
                .Where(cat => cat.Level == 0)
                .ToListAsync();
        }

        public async Task<List<CategoryModel>> GetSubCategoriesAsync(string parentId)
        {
            if (string.IsNullOrWhiteSpace(parentId) && string.IsNullOrWhiteSpace(_currentUser.UserId))
                throw new EndpointException("Parent ID and Owner ID cannot be empty");

            var categoriesQuery = await _repository.GetAllAsync();
            return await categoriesQuery
                .Where(category => category.ParentCategoryId == parentId)
                .OrderByDescending(cat => cat.LastEditedOnDate)
                .ToListAsync();
        }

        public async Task AddCategoryAsync(CategoryDto categoryInput)
        {
            if (string.IsNullOrWhiteSpace(categoryInput.Name))
                throw new EndpointException("Name cannot be empty");

            var doesExistQuery = await _repository.GetAllAsync();
            var exists = await doesExistQuery.AnyAsync(cat =>
                cat.Name == categoryInput.Name
                && cat.ParentCategoryId == categoryInput.CategoryParentId);

            if (exists)
                throw new EndpointException("This category already exists");

            CategoryModel parent;
            string path = _currentUser.UserId;
            int level = 0;

            if (!string.IsNullOrEmpty(categoryInput.CategoryParentId))
            {
                parent = await GetCategoryAsync(categoryInput.CategoryParentId);

                if (parent is null)
                    throw new EndpointException("This parent category doesn't exist");

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
                throw new EndpointException("Name cannot be empty");

            // check if under parent the same category name already exists
            if (!string.IsNullOrEmpty(categoryInput.CategoryParentId))
            {
                var categoryCheckQuery = await _repository.GetAllAsync();
                var isNameDuplicate = await categoryCheckQuery.AnyAsync(entry =>
                        entry.ParentCategoryId == categoryInput.CategoryParentId
                        && entry.Name == categoryInput.Name);

                if (isNameDuplicate)
                    throw new EndpointException("Couldn't update that category, this name is already being used");
            }

            // check if this category exists
            var categoryQuery = await _repository.GetAllAsync();
            var category = await categoryQuery.FirstOrDefaultAsync(cat =>
                cat.Id == categoryInput.CategoryId
                && cat.ParentCategoryId == categoryInput.CategoryParentId);

            if (category is null)
                throw new EndpointException("Couldn't update that category");

            category.Name = categoryInput.Name;
            category.LastEditedOnDate = DateTime.UtcNow;

            await _repository.UpdateAsync(category);
            await _repository.SaveChangesAsync();
        }

        public async Task RemoveCategoryAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new EndpointException("Name cannot be empty");

            var category = await _repository.GetAsync(id);
            if (category is null)
                throw new EndpointException("Couldn't find that category");

            await _repository.RemoveAsync(category);
            await _repository.SaveChangesAsync();
        }

        public async Task<CategoryModel> GetCategoryWithEntriesAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new EndpointException("Category ID cannot be empty");

            var categoryWithEntriesQuery = await _repository.GetAllAsync();
            var categoryWithEntries = await categoryWithEntriesQuery
                .Include(cat => cat.Entries.OrderByDescending(e => e.LastUpdatedOn))
                .FirstOrDefaultAsync(cat => cat.Id == id);

            return categoryWithEntries;
        }
    }
}