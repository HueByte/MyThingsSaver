using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Extensions;
using Common.ApiResonse;
using Common.Events;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

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
                { return await _categoryRepository.GetOneAsync(name, userId); });

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
    }
}