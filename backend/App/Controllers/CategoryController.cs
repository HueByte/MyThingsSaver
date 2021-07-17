using System;
using System.Threading.Tasks;
using App.Extensions;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddCategory([FromBody] CategoryDTO category)
        {
            var newCat = new Category() {
                name = "as"
            };

            _categoryRepository.AddOne(newCat);

            return Ok();
        }

    }
}