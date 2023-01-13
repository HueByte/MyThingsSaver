using Core.DTO;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Core.lib;

namespace MTS.App.Controllers
{
    [Authorize]
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
        [ProducesResponseType(200)]
        public async Task<IActionResult> SetUserAvatar(UserAvatarDto userAvatarDto)
        {
            _ = await _userInfoService.ChangeUserAvatarAsync(userAvatarDto?.AvatarUrl!);

            return ApiResponse.Empty();
        }

        [HttpPost("username")]
        public async Task<IActionResult> SetUsername(ChangeUsernameDto userUsernameDto)
        {
            _ = await _userInfoService.ChangeUsernameAsync(userUsernameDto?.Username!);

            return ApiResponse.Empty();
        }
    }
}