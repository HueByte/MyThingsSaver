using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Common.ApiResonse;
using MTS.Core.DTO;
using MTS.Core.lib;

namespace MTS.App.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthController(IUserService userService, IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("register")]
        [ProducesResponseType(200)]
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

        [HttpPost("changePassword")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto user)
        {
            await _userService.ChangePasswordAsync(user);

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
        [ProducesResponseType(200)]
        public async Task<IActionResult> RevokeToken([FromBody] string bodyToken)
        {
            var token = bodyToken ?? Request.Cookies[CookieNames.RefreshTokenCookie];

            await _refreshTokenService.RevokeToken(token!, GetIpAddress());

            return ApiResponse.Empty();
        }

        [HttpPost("logout")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies[CookieNames.RefreshTokenCookie];

            if (refreshToken is not null)
                await _refreshTokenService.RevokeToken(refreshToken!, GetIpAddress());

            Response.Cookies.Delete(CookieNames.RefreshTokenCookie);
            Response.Cookies.Delete(CookieNames.AccessToken);

            return ApiResponse.Empty();
        }

        private void AttachAuthCookies(VerifiedUserDto user)
        {
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

            Response.Cookies.Append(CookieNames.RefreshTokenCookie, user.RefreshToken, refreshTokenOptions);
            Response.Cookies.Append(CookieNames.AccessToken, user.Token, jwtTokenOptions);
        }

        private string GetIpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection!.RemoteIpAddress!.MapToIPv4().ToString();
        }
    }
}