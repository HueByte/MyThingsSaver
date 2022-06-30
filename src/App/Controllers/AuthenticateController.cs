using System;
using System.Threading.Tasks;
using App.Authentication;
using App.Extensions;
using Common.ApiResonse;
using Common.Constants;
using Core.DTO;
using Core.Entities;
using Core.lib;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class AuthenticateController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthenticateController(IUserService userService, IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerUser)
        {
            var data = await _userService.CreateUser(registerUser);

            return CreateResponse.FromData(data);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            var data = await _userService.LoginUser(userDto, GetIpAddress());
            var result = new BaseApiResponse<VerifiedUserDto>(data);

            if (result.IsSuccess)
            {
                AttachAuthCookies(result.Data!);
            }

            return CreateResponse.FromBaseApiResponse(result);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto user)
        {
            await _userService.ChangePasswordAsync(user);

            return CreateResponse.Empty();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies[CookieNames.RefreshTokenCookie];
            var data = await _refreshTokenService.RefreshToken(refreshToken!, GetIpAddress());

            var result = new BaseApiResponse<VerifiedUserDto>(data);

            if (result.IsSuccess && !string.IsNullOrEmpty(result.Data?.RefreshToken))
            {
                AttachAuthCookies(result.Data!);
            }

            return CreateResponse.FromBaseApiResponse(result);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] string bodyToken)
        {
            var token = bodyToken ?? Request.Cookies[CookieNames.RefreshTokenCookie];

            await _refreshTokenService.RevokeToken(token!, GetIpAddress());

            return CreateResponse.Empty();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies[CookieNames.RefreshTokenCookie];
            await _refreshTokenService.RevokeToken(refreshToken!, GetIpAddress());

            Response.Cookies.Delete(CookieNames.RefreshTokenCookie);
            Response.Cookies.Delete(CookieNames.AccessToken);
            return CreateResponse.Empty();
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