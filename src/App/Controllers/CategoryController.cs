using App.Extensions;
using Core.DTO;
using Core.Interfaces.Services;
using Core.lib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json.Serialization;

namespace App.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _categoryService.GetAllCategoriesAsync();

            return CreateResponse.FromData(data);
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetCategory(string id)
        {
            var data = await _categoryService.GetCategoryAsync(id);

            return CreateResponse.FromData(data);
        }

        [HttpGet("GetAllRoot")]
        [Authorize]
        public async Task<IActionResult> GetAllRoot()
        {
            var data = await _categoryService.GetRootCategoriesAsync();

            return CreateResponse.FromData(data);
        }

        [HttpGet("GetAllSub")]
        [Authorize]
        public async Task<IActionResult> GetAllSub([FromQuery] string parentId)
        {
            var data = await _categoryService.GetSubCategoriesAsync(parentId);

            return CreateResponse.FromData(data);
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            await _categoryService.AddCategoryAsync(category);

            return CreateResponse.Empty();
        }

        [HttpPost("Remove")]
        [Authorize]
        public async Task<IActionResult> RemoveCategoryAsync([FromBody] CategoryDto category)
        {
            await _categoryService.RemoveCategoryAsync(category.CategoryId);

            return CreateResponse.Empty();
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Updatecategory([FromBody] CategoryDto category)
        {
            await _categoryService.UpdateCategoryAsync(category);

            return CreateResponse.Empty();
        }

        [HttpGet("GetWithEntries")]
        [Authorize]
        public async Task<IActionResult> GetCategoryWithEntries([FromQuery] string categoryId)
        {
            var data = await _categoryService.GetCategoryWithEntriesAsync(categoryId);

            return CreateResponse.FromData(data);
        }
    }
}