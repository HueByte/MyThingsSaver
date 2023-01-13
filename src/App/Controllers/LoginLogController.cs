using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
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
        public async Task<IActionResult> GetAll()
        {
            var logs = await _loginLogService.GetAllLoginLogsAsync();

            return ApiResponse.Data(logs);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _loginLogService.GetLoginLogsCountAsync();

            return ApiResponse.ValueType(count);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetPaginated([FromQuery] int page, [FromQuery] int pageSize)
        {
            var logs = await _loginLogService.GetLoginLogsPaginatedAsync(page, pageSize);

            return ApiResponse.Data(logs);
        }
    }
}