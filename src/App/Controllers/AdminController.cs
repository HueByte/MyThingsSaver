using Core.DTO;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Common.ApiResonse;
using MTS.Core.lib;

namespace App.Controllers
{
    [Authorize(Roles = Role.ADMIN)]
    public class AdminController : BaseApiController
    {
        private readonly ILoginLogService _loginLogService;
        public AdminController(ILoginLogService loginLogService)
        {
            _loginLogService = loginLogService;
        }

        [HttpGet("loginLogsCount")]
        [ProducesResponseType(typeof(BaseApiResponse<int>), 200)]
        public async Task<IActionResult> GetLoginLogsCount()
        {
            var count = await _loginLogService.GetLoginLogsCountAsync();

            return ApiResponse.ValueType(count);
        }


        [HttpGet("loginLogs")]
        [ProducesResponseType(typeof(BaseApiResponse<List<LoginLogsDto>>), 200)]
        public async Task<IActionResult> GetLoginLogsPaginated([FromQuery] int page, [FromQuery] int pageSize)
        {
            var logs = await _loginLogService.GetLoginLogsPaginatedAsync(page, pageSize);

            return ApiResponse.Data(logs);
        }
    }
}