using App.Extensions;
using Common.ApiResonse;
using Core.DTO;
using Core.Interfaces.Services;
using Core.lib;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Authorize]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponse<CategoryModel>), 200)]
        public async Task<IActionResult> GetCategory(string id)
        {
            var data = await _categoryService.GetCategoryAsync(id);

            return CreateResponse.FromData(data);
        }

        [HttpGet("All")]
        [ProducesResponseType(typeof(BaseApiResponse<List<CategoryModel>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _categoryService.GetAllCategoriesAsync();

            return CreateResponse.FromData(data);
        }

        [HttpGet("AllRoot")]
        [ProducesResponseType(typeof(BaseApiResponse<List<CategoryModel>>), 200)]
        public async Task<IActionResult> GetAllRoot()
        {
            var data = await _categoryService.GetRootCategoriesAsync();

            return CreateResponse.FromData(data);
        }

        [HttpGet("AllSub")]
        [ProducesResponseType(typeof(BaseApiResponse<List<CategoryModel>>), 200)]
        public async Task<IActionResult> GetAllSub([FromQuery] string parentId)
        {
            var data = await _categoryService.GetSubCategoriesAsync(parentId);

            return CreateResponse.FromData(data);
        }

        [HttpGet("WithEntries")]
        [ProducesResponseType(typeof(BaseApiResponse<CategoryModel>), 200)]
        public async Task<IActionResult> GetCategoryWithEntries([FromQuery] string categoryId)
        {
            var data = await _categoryService.GetCategoryWithEntriesAsync(categoryId);

            return CreateResponse.FromData(data);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            await _categoryService.AddCategoryAsync(category);

            return CreateResponse.Empty();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RemoveCategoryAsync([FromBody] CategoryDto category)
        {
            await _categoryService.RemoveCategoryAsync(category.CategoryId);

            return CreateResponse.Empty();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Updatecategory([FromBody] CategoryDto category)
        {
            await _categoryService.UpdateCategoryAsync(category);

            return CreateResponse.Empty();
        }
    }
}