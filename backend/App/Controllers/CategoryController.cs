using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Extensions;
using Common.Events;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace App.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost("AddCategory")]
        [Authorize]
        public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryDTO category)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryRepository.AddOneAsync(category, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);

        }

        [HttpGet("GetAllCategories")]
        [Authorize]
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<List<Category>>.EventHandleAsync(async () =>
                { return await _categoryRepository.GetAllAsync(userId); });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetCategory")]
        [Authorize]
        public async Task<IActionResult> GetCategoryAsync(string name)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<Category>.EventHandleAsync(async () =>
                { return await _categoryRepository.GetOneByIdAsync(name, userId); });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("RemoveCategory")]
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

        [HttpPost("UpdateCategory")]
        [Authorize]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] CategoryDTO category)
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

        // TODO: change response in .net 6
        [HttpGet("GetCategoryWithEntries")]
        [Authorize]
        public async Task<IActionResult> GetCategoryEntriesAsync([FromQuery] string categoryId)
        {
            var userid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<Category>.EventHandleAsync(async () =>
            {
                return await _categoryRepository.GetCategoryWithEntriesAsync(categoryId, userid);
            });

            // var options = new JsonSerializerOptions
            // {
            //     ReferenceHandler = ReferenceHandler.Preserve,
            //     WriteIndented = true
            // };

            // string testJson = JsonSerializer.Serialize(result, options);


            // TODO : Update once upgrade to .net 6, temporary solution to prevent JSON circular reference error
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

            string resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result, settings);

            if (result.IsSuccess)
                return Ok(resultJson);
            else
                return BadRequest(resultJson);
        }
    }
}