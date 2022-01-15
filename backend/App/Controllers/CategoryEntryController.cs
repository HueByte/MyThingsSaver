using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Extensions;
using Common.Events;
using Core.DTO;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json.Serialization;

namespace App.Controllers
{
    public class CategoryEntryController : BaseApiController
    {
        private readonly ICategoryEntryRepository _categoryEntryRepository;
        public CategoryEntryController(ICategoryEntryRepository categoryEntryRepository)
        {
            _categoryEntryRepository = categoryEntryRepository;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<CategoryEntry>.EventHandleAsync(async () =>
                await _categoryEntryRepository.GetOneByIdAsync(id, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string categoryId, bool withContent)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<AllCategoryEntries>.EventHandleAsync(async () =>
                await _categoryEntryRepository.GetAllAsync(categoryId, userId, withContent));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] CategoryEntryDTO entry)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.AddOneAsync(entry, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] CategoryEntryDTO entry)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.UpdateOneAsync(entry, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("UpdateWithoutContent")]
        [Authorize]
        public async Task<IActionResult> UpdateWithoutContent([FromBody] CategoryEntryDTO entry)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.UpdateOneWithoutContentAsync(entry, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.RemoveOneAsync(id, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetRecent")]
        [Authorize]
        public async Task<IActionResult> GetRecent()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<List<CategoryEntry>>.EventHandleAsync(async () =>
                await _categoryEntryRepository.GetRecentAsync(userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}