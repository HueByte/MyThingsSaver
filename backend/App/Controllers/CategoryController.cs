using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using App.Extensions;
using Common.ApiResonse;
using Common.Events;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace App.Controllers
{
    public class CategoryController : BaseApiController
    {
        readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost("/AddCategory")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryDTO category)
        {
            var newCategory = new Category()
            {
                name = category.Name,
                DateCreated = DateTime.UtcNow,
                CategoryId = Guid.NewGuid()
            };

            var result = await ApiEventHandler.EventHandleAsync(async () => { await _categoryRepository.AddOneAsync(newCategory); });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);

        }

        [HttpGet("/GetAllCategory")]
        public async Task<IActionResult> GetAllCategoryAsync()
        {

            var result = await ApiEventHandler<List<Category>>.EventHandleAsync(async () => { return await _categoryRepository.GetAllAsync(); });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("/GetCategory")]
        public async Task<IActionResult> GetCategoryAsync(string name)
        {
            var result = await ApiEventHandler<Category>.EventHandleAsync(async () => { return await _categoryRepository.GetOneAsync(name); });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}