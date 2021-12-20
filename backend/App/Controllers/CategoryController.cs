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
        [Authorize]
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

        // TODO: change response in .net 6
        [HttpGet("GetWithEntries")]
        [Authorize]
        public async Task<IActionResult> GetCategoryWithEntries([FromQuery] string categoryId)
        {
            var userid = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<Category>.EventHandleAsync(async () =>
                await _categoryRepository.GetCategoryWithEntriesAsync(categoryId, userid));

            // TODO : Update once upgrade to .net 6, temporary solution to prevent JSON circular reference error
            // var settings = new Newtonsoft.Json.JsonSerializerSettings
            // {
            //     ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //     ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            //     Formatting = Newtonsoft.Json.Formatting.Indented
            // };

            // string resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result, settings);

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}