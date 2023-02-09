using Core.DTO;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using MTS.App.Extensions;
using MTS.Common.ApiResonse;
using MTS.Core.DTO;
using MTS.Core.Entities;
using MTS.Core.lib;
using MTS.Core.Services.CurrentUser;

namespace MTS.App.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ICurrentUserService _currentUser;
        public AuthController(IUserService userService, IRefreshTokenService refreshTokenService, ICurrentUserService currentUser)
        {
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _currentUser = currentUser;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(BaseApiResponse<IdentityResult>), 200)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerUser)
        {
            var data = await _userService.CreateUser(registerUser);

            return ApiResponse.Data(data);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(BaseApiResponse<VerifiedUserDto>), 200)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            var data = await _userService.LoginUser(userDto, GetIpAddress());
            var result = new BaseApiResponse<VerifiedUserDto>(data);

            if (result.IsSuccess)
            {
                AttachAuthCookies(result.Data!);
            }

            return ApiResponse.Create(result);
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(BaseApiResponse<UserInfoDto>), 200)]
        [Authorize]
        public async Task<IActionResult> GetUserInformation()
        {
            var result = await _userService.GetUserInfoByIdAsync(_currentUser.UserId);

            return ApiResponse.Data(result);
        }

        [HttpGet("user")]
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> GetUserInformation([FromQuery] string userId = "", [FromQuery] string email = "", [FromQuery] string username = "")
        {
            UserInfoDto? result;
            if (string.IsNullOrEmpty(userId))
            {
                result = await _userService.GetUserInfoByIdAsync(userId);
            }
            else if (string.IsNullOrEmpty(email))
            {
                result = await _userService.GetUserInfoByEmailAsync(email);
            }
            else if (string.IsNullOrEmpty(username))
            {
                result = await _userService.GetUserInfoByUsernameAsync(username);
            }
            else throw new HandledException("No user id, email or username provided");

            return ApiResponse.Data(result);
        }

        [HttpPost("avatar")]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        [Authorize]
        public async Task<IActionResult> SetUserAvatar([FromBody] UserAvatarDto userAvatarDto)
        {
            _ = await _userService.ChangeUserAvatarAsync(userAvatarDto?.AvatarUrl!);

            return ApiResponse.Empty();
        }

        [HttpPost("username")]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        [Authorize]
        public async Task<IActionResult> SetUsername([FromBody] ChangeUsernameDto userUsernameDto)
        {
            _ = await _userService.ChangeUsernameAsync(userUsernameDto?.Username!, userUsernameDto?.Password!);

            return ApiResponse.Empty();
        }

        [HttpPost("password")]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        [Authorize]
        public async Task<IActionResult> SetPassword([FromBody] ChangePasswordDto userPasswordDto)
        {
            _ = await _userService.ChangePasswordAsync(userPasswordDto?.CurrentPassword!, userPasswordDto?.NewPassword!);

            return ApiResponse.Empty();
        }

        [HttpPost("refreshToken")]
        [ProducesResponseType(typeof(BaseApiResponse<VerifiedUserDto>), 200)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies[CookieNames.RefreshTokenCookie];
            var data = await _refreshTokenService.RefreshToken(refreshToken!, GetIpAddress());

            var result = new BaseApiResponse<VerifiedUserDto>(data);

            if (result.IsSuccess && !string.IsNullOrEmpty(result.Data?.RefreshToken))
            {
                AttachAuthCookies(result.Data!);
            }

            return ApiResponse.Create(result);
        }

        [HttpPost("revokeToken")]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        public async Task<IActionResult> RevokeToken([FromBody] string bodyToken)
        {
            var token = bodyToken ?? Request.Cookies[CookieNames.RefreshTokenCookie];

            await _refreshTokenService.RevokeToken(token!, GetIpAddress());

            return ApiResponse.Empty();
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies[CookieNames.RefreshTokenCookie];

            if (refreshToken is not null)
                await _refreshTokenService.RevokeToken(refreshToken!, GetIpAddress());

            Response.Cookies.Delete(CookieNames.RefreshTokenCookie);
            Response.Cookies.Delete(CookieNames.AccessToken);

            return ApiResponse.Empty();
        }

        [HttpPost("email")]
        [ProducesResponseType(typeof(BaseApiResponse<object>), 200)]
        public async Task<IActionResult> ChangeEmail(ChangeEmailDto emailDto)
        {
            var result = await _userService.ChangeEmailAsync(emailDto?.Email!, emailDto?.Password!);
            BaseApiResponse<object> response = new() { IsSuccess = result };

            return ApiResponse.Create(response);
        }

        private void AttachAuthCookies(VerifiedUserDto user)
        {
            if (user is null) return;

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = user.RefreshTokenExpiration
            };

            var jwtTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = user.AccessTokenExpiration,
            };

            Response.Cookies.Append(CookieNames.RefreshTokenCookie, user.RefreshToken!, refreshTokenOptions);
            Response.Cookies.Append(CookieNames.AccessToken, user.Token!, jwtTokenOptions);
        }

        private string GetIpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request?.Headers["X-Forwarded-For"]!;
            else
                return HttpContext.Connection!.RemoteIpAddress!.MapToIPv4().ToString();
        }
    }
}