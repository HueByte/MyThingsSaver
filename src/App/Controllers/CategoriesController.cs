using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Common.ApiResonse;
using MTS.Core.DTO;
using MTS.Core.lib;
using MTS.Core.Models;

namespace MTS.App.Controllers
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

            return ApiResponse.Data(data);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(BaseApiResponse<List<CategoryModel>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _categoryService.GetAllCategoriesAsync();

            return ApiResponse.Data(data);
        }

        [HttpGet("allRoot")]
        [ProducesResponseType(typeof(BaseApiResponse<List<CategoryModel>>), 200)]
        public async Task<IActionResult> GetAllRoot()
        {
            var data = await _categoryService.GetRootCategoriesAsync();

            return ApiResponse.Data(data);
        }

        [HttpGet("allSub")]
        [ProducesResponseType(typeof(BaseApiResponse<List<CategoryModel>>), 200)]
        public async Task<IActionResult> GetAllSub([FromQuery] string parentId)
        {
            var data = await _categoryService.GetSubCategoriesAsync(parentId);

            return ApiResponse.Data(data);
        }

        [HttpGet("withEntries")]
        [ProducesResponseType(typeof(BaseApiResponse<CategoryModel>), 200)]
        public async Task<IActionResult> GetCategoryWithEntries([FromQuery] string categoryId)
        {
            var data = await _categoryService.GetCategoryWithEntriesAsync(categoryId);

            return ApiResponse.Data(data);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            await _categoryService.AddCategoryAsync(category);

            return ApiResponse.Empty();
        }

        [HttpDelete]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        public async Task<IActionResult> RemoveCategoryAsync([FromBody] CategoryDto category)
        {
            await _categoryService.RemoveCategoryAsync(category.CategoryId!);

            return ApiResponse.Empty();
        }

        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto category)
        {
            await _categoryService.UpdateCategoryAsync(category);

            return ApiResponse.Empty();
        }
    }
}