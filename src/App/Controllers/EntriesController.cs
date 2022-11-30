using App.Extensions;
using Core.DTO;
using Core.Interfaces.Services;
using Core.lib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// using Newtonsoft.Json.Serialization;

namespace App.Controllers
{
    [Authorize]
    public class EntriesController : BaseApiController
    {
        private readonly IEntryService _entryService;
        public EntriesController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var data = await _entryService.GetEntryAsync(id);

            return CreateResponse.FromData(data);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll([FromQuery] string categoryId, bool withContent)
        {
            var data = await _entryService.GetAllEntriesAsync(categoryId, withContent);

            return CreateResponse.FromData(data);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EntryDTO entry)
        {
            await _entryService.AddEntryAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EntryDTO entry)
        {
            await _entryService.UpdateEntryAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateWithoutContent([FromBody] EntryDTO entry)
        {
            await _entryService.UpdateEntryWithoutContentAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            await _entryService.RemoveEntryAsync(id);

            return CreateResponse.Empty();
        }

        [HttpGet("Recent")]
        public async Task<IActionResult> GetRecent()
        {
            var data = await _entryService.GetRecentAsync();

            return CreateResponse.FromData(data);
        }
    }
}