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
        private readonly IUserService _userService;
        public AdminController(ILoginLogService loginLogService, IUserService userService)
        {
            _loginLogService = loginLogService;
            _userService = userService;
        }

        [HttpGet("managementUsers")]
        [ProducesResponseType(typeof(BaseApiResponse<List<ManagementUserDto>>), 200)]
        public async Task<IActionResult> GetManagementUsers()
        {
            var users = await _userService.GetManagementUsers();

            return ApiResponse.Data(users);
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