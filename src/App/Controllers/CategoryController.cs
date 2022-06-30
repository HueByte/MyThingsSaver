using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Extensions;
using Common.Events;
using Core.DTO;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json.Serialization;

namespace App.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll()
        {
            var result = await ApiEventHandler<List<Category>>.EventHandleAsync(async () =>
                await _categoryRepository.GetAllAsync());

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetCategory(string id)
        {
            var result = await ApiEventHandler<Category>.EventHandleAsync(async () =>
                await _categoryRepository.GetOneByIdAsync(id));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetAllRoot")]
        [Authorize]
        public async Task<IActionResult> GetAllRoot()
        {
            var result = await ApiEventHandler<List<Category>>.EventHandleAsync(async () =>
                await _categoryRepository.GetRootCategoriesAsync());

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetAllSub")]
        [Authorize]
        public async Task<IActionResult> GetAllSub([FromQuery] string parentId)
        {
            var result = await ApiEventHandler<List<Category>>.EventHandleAsync(async () =>
                await _categoryRepository.GetSubcategoriesAsync(parentId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryRepository.AddOneAsync(category));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);

        }

        [HttpPost("Remove")]
        [Authorize]
        public async Task<IActionResult> RemoveCategoryAsync([FromBody] CategoryDto category)
        {
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryRepository.RemoveOneAsync(category.CategoryId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Updatecategory([FromBody] CategoryDto category)
        {
            var result = await ApiEventHandler.EventHandleAsync(async () =>
            {
                await _categoryRepository.UpdateOneAsync(category);
            });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetWithEntries")]
        [Authorize]
        public async Task<IActionResult> GetCategoryWithEntries([FromQuery] string categoryId)
        {
            var result = await ApiEventHandler<Category>.EventHandleAsync(async () =>
                await _categoryRepository.GetCategoryWithEntriesAsync(categoryId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // temp
        [HttpGet("test")]
        [Authorize]
        public async Task<IActionResult> Test([FromQuery] string name, string parentid)
        {
            // root ID - 08992e73-57f8-4472-8a2b-131e93aabf7a
            // var rootID = "2a09cd6e-f9a0-4e2e-a4a5-58c2dea12395";

            var category = new CategoryDto()
            {
                CategoryParentId = parentid,
                Name = name
            };

            var username = this.User.FindFirst(ClaimTypes.Name).Value;

            // await _categoryRepository.AddOneAsync(category, );

            // update paths
            // var allCategories = await _categoryRepository.GetAllAsync();

            // var roots = allCategories.Where(x => x.ParentCategoryId == null).ToList();

            // // update roots 
            // roots.ForEach(cat =>
            // {
            //     cat.Path = $"{}/{cat.CategoryId}";
            //     cat.Level = 0;
            // });

            // var subs = allCategories.Where(x => !roots.Any(z => z.CategoryId == x.CategoryId)).ToList();

            // // update subs
            // subs.ForEach(cat =>
            // {
            //     cat.Path = $"{}/{cat.ParentCategoryId}/{cat.CategoryId}";
            //     cat.Level = 1;
            // });

            // await _categoryRepository.UpdateMultipleAsync(roots, );
            // await _categoryRepository.UpdateMultipleAsync(subs, );

            return Ok();
        }
    }
}