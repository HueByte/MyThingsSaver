using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Core.lib;

namespace MTS.App.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUserInfoService _userInfoService;
        public UserController(IUserInfoService userInfoService)
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
    }
}