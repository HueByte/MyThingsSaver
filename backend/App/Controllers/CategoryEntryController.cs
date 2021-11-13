using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Extensions;
using Common.Events;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

namespace App.Controllers
{
    public class CategoryEntryController : BaseApiController
    {
        private readonly ICategoryEntryRepository _categoryEntryRepository;
        public CategoryEntryController(ICategoryEntryRepository categoryEntryRepository)
        {
            _categoryEntryRepository = categoryEntryRepository;
        }

        [HttpGet("GetEntryById")]
        [Authorize]
        public async Task<IActionResult> GetEntryByIdAsync(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<CategoryEntry>.EventHandleAsync(async () =>
                await _categoryEntryRepository.GetOneByIdAsync(id, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetAllEntries")]
        [Authorize]
        public async Task<IActionResult> GetAllEntries([FromQuery] string categoryId, bool withContent)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<AllCategoryEntries>.EventHandleAsync(async () =>
                await _categoryEntryRepository.GetAllAsync(categoryId, userId, withContent));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
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

        [HttpPost("UpdateEntry")]
        [Authorize]
        public async Task<IActionResult> UpdateOneEntry([FromBody] CategoryEntryDTO entry)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.UpdateOneAsync(entry, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpDelete("DeleteEntry")]
        [Authorize]
        public async Task<IActionResult> DeleteEntryAsync([FromQuery] string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.RemoveOneAsync(id, userId));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // TODO: change response in .net 6
        [HttpGet("GetRecent")]
        [Authorize]
        public async Task<IActionResult> GetRecentAsync()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await ApiEventHandler<List<CategoryEntry>>.EventHandleAsync(async () =>
                await _categoryEntryRepository.GetRecentAsync(userId));

            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

            string resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result, settings);

            if (result.IsSuccess)
                return Ok(resultJson);
            else
                return BadRequest(resultJson);
        }
    }
}