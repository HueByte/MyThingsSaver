using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
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

        [HttpGet("getLoginLogsCount")]
        public async Task<IActionResult> GetLoginLogsCount()
        {
            var count = await _loginLogService.GetLoginLogsCountAsync();

            return ApiResponse.ValueType(count);
        }


        [HttpGet("loginLogs")]
        public async Task<IActionResult> GetLoginLogsPaginated([FromQuery] int page, [FromQuery] int pageSize)
        {
            var logs = await _loginLogService.GetLoginLogsPaginatedAsync(page, pageSize);

            return ApiResponse.Data(logs);
        }
    }
}