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
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<List<Category>>.EventHandleAsync(async () =>
                await _categoryRepository.GetAllAsync(userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetCategory(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<Category>.EventHandleAsync(async () =>
                await _categoryRepository.GetOneByIdAsync(id, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO category)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryRepository.AddOneAsync(category, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);

        }

        [HttpPost("Remove")]
        [Authorize]
        public async Task<IActionResult> RemoveCategoryAsync([FromBody] CategoryDTO category)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryRepository.RemoveOneAsync(category.CategoryId, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Updatecategory([FromBody] CategoryDTO category)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
            {
                await _categoryRepository.UpdateOneAsync(category, userId);
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
            var userid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<Category>.EventHandleAsync(async () =>
                await _categoryRepository.GetCategoryWithEntriesAsync(categoryId, userid));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("test")]
        [Authorize]
        public async Task<IActionResult> Test([FromQuery] string name)
        {
            // root ID - 08992e73-57f8-4472-8a2b-131e93aabf7a
            var rootID = "2a09cd6e-f9a0-4e2e-a4a5-58c2dea12395";

            var category = new CategoryDTO()
            {
                CategoryParentId = rootID,
                Name = name
            };

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await _categoryRepository.AddOneAsync(category, userId);

            return Ok();
        }
    }
}