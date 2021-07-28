using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Extensions;
using Common.Events;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualBasic;

namespace App.Controllers
{
    public class CategoryEntryController : BaseApiController
    {
        private readonly ICategoryEntryRepository _categoryEntryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public CategoryEntryController(ICategoryEntryRepository categoryEntryRepository, ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager)
        {
            _categoryEntryRepository = categoryEntryRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }

        [HttpPost("AddEntry")]
        [Authorize]
        public async Task<IActionResult> AddEntryAsync([FromBody] CategoryEntryDTO entry)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.AddOneAsync(entry, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetEntryByName")]
        [Authorize]
        public async Task<IActionResult> GetEntryByNameAsync(string categoryId, string name)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<CategoryEntry>.EventHandleAsync(async () =>
            {
                return await _categoryEntryRepository.GetOneByNameAsync(categoryId, name, userId);
            });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetEntryById")]
        [Authorize]
        public async Task<IActionResult> GetEntryByIdAsync(string categoryId, string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<CategoryEntry>.EventHandleAsync(async () =>
            {
                return await _categoryEntryRepository.GetOneByIdAsync(categoryId, id, userId);
            });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetAllEntries")]
        [Authorize]
        public async Task<IActionResult> GetAllEntries(string categoryId)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<List<CategoryEntry>>.EventHandleAsync(async () =>
            {
                return await _categoryEntryRepository.GetAllAsync(categoryId, userId);
            });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}