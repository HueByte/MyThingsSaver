using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Extensions;
using Common.ApiResonse;
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
            var result = await ApiEventHandler<CategoryEntry>.EventHandleAsync(async () =>
                await _categoryEntryRepository.GetOneByIdAsync(id));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string categoryId, bool withContent)
        {

            var result = await _categoryEntryRepository.GetAllAsync(categoryId, withContent);
            var response = new BaseApiResponse<AllCategoryEntries>(result);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] CategoryEntryDto entry)
        {
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.AddOneAsync(entry));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] CategoryEntryDto entry)
        {
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.UpdateOneAsync(entry));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("UpdateWithoutContent")]
        [Authorize]
        public async Task<IActionResult> UpdateWithoutContent([FromBody] CategoryEntryDto entry)
        {
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.UpdateOneWithoutContentAsync(entry));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _categoryEntryRepository.RemoveOneAsync(id));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpGet("GetRecent")]
        [Authorize]
        public async Task<IActionResult> GetRecent()
        {
            var result = await ApiEventHandler<List<CategoryEntry>>.EventHandleAsync(async () =>
                await _categoryEntryRepository.GetRecentAsync());

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}