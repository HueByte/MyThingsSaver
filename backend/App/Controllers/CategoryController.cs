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

        // [HttpPost("/AddCategory")]
        // public async Task<IActionResult> AddCategory([FromBody] CategoryDTO category)
        // {
        //     var newCat = new Category() {

        //     };

        //     return Ok();
        // }

        [HttpPost("/AddCategory")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryDTO category)
        {
            BaseApiResponse<string> response;

            var newCategory = new Category()
            {
                name = category.Name,
                DateCreated = DateTime.UtcNow,
                CategoryId = Guid.NewGuid()
            };

            try
            {
                await _categoryRepository.AddOneAsync(newCategory);
                response = new BaseApiResponse<string>()
                {
                    Data = null,
                    Errors = null,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                response = new BaseApiResponse<string>()
                {
                    Data = "Something went wrong",
                    Errors = new List<string>() { e.Message }
                };

                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("/GetAllCategory")]
        public async Task<IActionResult> GetAllCategoryAsync()
        {
            BaseApiResponse<List<Category>> response;

            try
            {
                var categories = await _categoryRepository.GetAllAsync();

                response = new BaseApiResponse<List<Category>>()
                {
                    Data = categories,
                    Errors = null,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                response = new BaseApiResponse<List<Category>>()
                {
                    Data = null,
                    Errors = new List<string>() { e.Message },
                    IsSuccess = false
                };

                return BadRequest(response);
            }

            return Ok(response);
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