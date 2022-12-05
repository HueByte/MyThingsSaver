using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Common.ApiResonse;
using MTS.Core.DTO;
using MTS.Core.Interfaces.Services;
using MTS.Core.Models;
using MTS.Core.lib;
// using Newtonsoft.Json.Serialization;

namespace MTS.App.Controllers
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
        [ProducesResponseType(typeof(BaseApiResponse<EntryModel>), 200)]
        public async Task<IActionResult> Get([FromQuery] string id)
        {
            var data = await _entryService.GetEntryAsync(id);

            return CreateResponse.FromData(data);
        }

        [HttpGet("All")]
        [ProducesResponseType(typeof(BaseApiResponse<List<EntryModel>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] string categoryId, bool withContent)
        {
            var data = await _entryService.GetAllEntriesAsync(categoryId, withContent);

            return CreateResponse.FromData(data);
        }

        [HttpGet("Recent")]
        [ProducesResponseType(typeof(BaseApiResponse<List<EntryModel>>), 200)]
        public async Task<IActionResult> GetRecent()
        {
            var data = await _entryService.GetRecentAsync();

            return CreateResponse.FromData(data);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Add([FromBody] EntryDTO entry)
        {
            await _entryService.AddEntryAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update([FromBody] EntryDTO entry)
        {
            await _entryService.UpdateEntryAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpPatch]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateWithoutContent([FromBody] EntryDTO entry)
        {
            await _entryService.UpdateEntryWithoutContentAsync(entry);

            return CreateResponse.Empty();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            await _entryService.RemoveEntryAsync(id);

            return CreateResponse.Empty();
        }
    }
}