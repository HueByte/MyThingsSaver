using System.Formats.Asn1;
using Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Common.ApiResonse;
using MTS.Core.DTO;
using MTS.Core.lib;

namespace MTS.App.Controllers
{
    public class EntriesController : BaseApiController
    {
        private readonly IEntryService _entryService;
        private readonly IPublicEntryService _publicEntryService;
        public EntriesController(IEntryService entryService, IPublicEntryService publicEntryService)
        {
            _entryService = entryService;
            _publicEntryService = publicEntryService;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(BaseApiResponse<EntryModel>), 200)]
        public async Task<IActionResult> Get([FromQuery] string id)
        {
            var data = await _entryService.GetEntryAsync(id);

            return ApiResponse.Data(data);
        }

        [HttpGet("all")]
        [Authorize]
        [ProducesResponseType(typeof(BaseApiResponse<List<EntryModel>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] string categoryId, bool withContent)
        {
            var data = await _entryService.GetAllEntriesAsync(categoryId, withContent);

            return ApiResponse.Data(data);
        }

        [HttpGet("recent")]
        [Authorize]
        [ProducesResponseType(typeof(BaseApiResponse<List<EntryModel>>), 200)]
        public async Task<IActionResult> GetRecent()
        {
            var data = await _entryService.GetRecentAsync();

            return ApiResponse.Data(data);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        public async Task<IActionResult> Add([FromBody] EntryDTO entry)
        {
            await _entryService.AddEntryAsync(entry);

            return ApiResponse.Empty();
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        public async Task<IActionResult> Update([FromBody] EntryDTO entry)
        {
            await _entryService.UpdateEntryAsync(entry);

            return ApiResponse.Empty();
        }

        [HttpPatch]
        [Authorize]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        public async Task<IActionResult> UpdateWithoutContent([FromBody] EntryDTO entry)
        {
            await _entryService.UpdateEntryWithoutContentAsync(entry);

            return ApiResponse.Empty();
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        public async Task<IActionResult> Delete([FromQuery] string id)
        {
            await _entryService.RemoveEntryAsync(id);

            return ApiResponse.Empty();
        }

        [HttpPatch("makePublic")]
        [Authorize]
        public async Task<IActionResult> MakePublic([FromBody] UpdatePublicEntryDto entry)
        {
            var result = await _publicEntryService.TogglePublicEntryAsync(entry.TargetId);

            return ApiResponse.Data(result);
        }

        [HttpGet("public")]
        public async Task<IActionResult> MakePublic([FromQuery] string url)
        {
            var result = await _publicEntryService.GetPublicEntryAsync(url);

            return ApiResponse.Data(result);
        }
    }
}