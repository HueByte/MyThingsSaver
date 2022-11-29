using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Extensions;
using Common.ApiResonse;
using Core.DTO;
using Core.Entities;
using Core.lib;
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
        private readonly ICategoryRepository2 _categoryRepository;
        public CategoryController(ICategoryRepository2 categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("GetAll")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _categoryRepository.GetAllAsync();

            return CreateResponse.FromData(data);
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetCategory(string id)
        {
            var data = await _categoryRepository.GetOneByIdAsync(id);

            return CreateResponse.FromData(data);
        }

        [HttpGet("GetAllRoot")]
        [Authorize]
        public async Task<IActionResult> GetAllRoot()
        {
            var data = await _categoryRepository.GetRootCategoriesAsync();

            return CreateResponse.FromData(data);
        }

        [HttpGet("GetAllSub")]
        [Authorize]
        public async Task<IActionResult> GetAllSub([FromQuery] string parentId)
        {
            var data = await _categoryRepository.GetSubcategoriesAsync(parentId);

            return CreateResponse.FromData(data);
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            await _categoryRepository.AddOneAsync(category);

            return CreateResponse.Empty();
        }

        [HttpPost("Remove")]
        [Authorize]
        public async Task<IActionResult> RemoveCategoryAsync([FromBody] CategoryDto category)
        {
            await _categoryRepository.RemoveOneAsync(category.CategoryId);

            return CreateResponse.Empty();
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Updatecategory([FromBody] CategoryDto category)
        {
            await _categoryRepository.UpdateOneAsync(category);

            return CreateResponse.Empty();
        }

        [HttpGet("GetWithEntries")]
        [Authorize]
        public async Task<IActionResult> GetCategoryWithEntries([FromQuery] string categoryId)
        {
            var data = await _categoryRepository.GetCategoryWithEntriesAsync(categoryId);

            return CreateResponse.FromData(data);
        }
    }
}