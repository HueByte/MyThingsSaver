using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Common.ApiResonse;
using MTS.Core.DTO;
using MTS.Core.lib;

namespace App.Controllers
{
    [Authorize]
    public class LoginLogController : BaseApiController
    {
        private readonly ILoginLogService _loginLogService;
        public LoginLogController(ILoginLogService loginLogService)
        {
            _loginLogService = loginLogService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponse<List<LoginLogModel>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _loginLogService.GetAllUserLoginLogsAsync();

            return ApiResponse.Data(logs);
        }

        [HttpGet("count")]
        [ProducesResponseType(typeof(BaseApiResponse<int>), 200)]
        public async Task<IActionResult> GetCount()
        {
            var count = await _loginLogService.GetUserLoginLogsCountAsync();

            return ApiResponse.ValueType(count);
        }

        [HttpGet("paginated")]
        [ProducesResponseType(typeof(BaseApiResponse<List<LoginLogModel>>), 200)]
        public async Task<IActionResult> GetPaginated([FromQuery] int page, [FromQuery] int pageSize)
        {
            var logs = await _loginLogService.GetUserLoginLogsPaginatedAsync(page, pageSize);

            return ApiResponse.Data(logs);
        }
    }
}