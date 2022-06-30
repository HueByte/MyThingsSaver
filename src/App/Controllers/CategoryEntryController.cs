using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Extensions;
using Common.ApiResonse;
using Common.Events;
using Core.DTO;
using Core.Entities;
using Core.lib;
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
            var data = await _categoryEntryRepository.GetOneByIdAsync(id);

            return CreateResponse.FromData(data);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string categoryId, bool withContent)
        {
            var data = await _categoryEntryRepository.GetAllAsync(categoryId, withContent);

            return CreateResponse.FromData(data);
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] CategoryEntryDto entry)
        {
            await _categoryEntryRepository.AddOneAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] CategoryEntryDto entry)
        {
            await _categoryEntryRepository.UpdateOneAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpPost("UpdateWithoutContent")]
        [Authorize]
        public async Task<IActionResult> UpdateWithoutContent([FromBody] CategoryEntryDto entry)
        {
            await _categoryEntryRepository.UpdateOneWithoutContentAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            await _categoryEntryRepository.RemoveOneAsync(id);

            return CreateResponse.Empty();
        }

        [HttpGet("GetRecent")]
        [Authorize]
        public async Task<IActionResult> GetRecent()
        {
            var data = await _categoryEntryRepository.GetRecentAsync();

            return CreateResponse.FromData(data);
        }
    }
}