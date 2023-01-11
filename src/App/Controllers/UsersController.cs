using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Core.lib;
using MTS.Core.Services.CurrentUser;

namespace MTS.App.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserInfoService _userInfoService;
        public UsersController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [HttpGet("me")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetUserInformation()
        {
            var result = await _userInfoService.GetUserInfoAsync();

            return ApiResponse.Data(result);
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> SetUserAvatar()
        {
            return ApiResponse.Empty();
        }
    }
}