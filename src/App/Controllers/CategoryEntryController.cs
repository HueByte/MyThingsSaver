using App.Extensions;
using Core.DTO;
using Core.Interfaces.Services;
using Core.lib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json.Serialization;

namespace App.Controllers
{
    public class CategoryEntryController : BaseApiController
    {
        private readonly IEntryService _entryService;
        public CategoryEntryController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet("Get")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await _entryService.GetEntryAsync(id);

            return CreateResponse.FromData(data);
        }

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string categoryId, bool withContent)
        {
            var data = await _entryService.GetAllEntriesAsync(categoryId, withContent);

            return CreateResponse.FromData(data);
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] EntryDTO entry)
        {
            await _entryService.AddEntryAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] EntryDTO entry)
        {
            await _entryService.UpdateEntryAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpPost("UpdateWithoutContent")]
        [Authorize]
        public async Task<IActionResult> UpdateWithoutContent([FromBody] EntryDTO entry)
        {
            await _entryService.UpdateEntryWithoutContentAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            await _entryService.RemoveEntryAsync(id);

            return CreateResponse.Empty();
        }

        [HttpGet("GetRecent")]
        [Authorize]
        public async Task<IActionResult> GetRecent()
        {
            var data = await _entryService.GetRecentAsync();

            return CreateResponse.FromData(data);
        }
    }
}