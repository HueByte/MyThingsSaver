using System;
using System.Threading.Tasks;
using App.Authentication;
using App.Extensions;
using Common.Constants;
using Common.Events;
using Core.DTO;
using Core.Entities;
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
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _userService.CreateUser(registerUser));

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            var result = await ApiEventHandler<VerifiedUserDto>.EventHandleAsync(async () =>
               await _userService.LoginUser(userDto, GetIpAddress()));

            if (result.IsSuccess)
            {
                AttachAuthCookies(result.Data!);
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto user)
        {
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _userService.ChangePasswordAsync(user));

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies[CookieNames.RefreshTokenCookie];
            var result = await ApiEventHandler<VerifiedUserDto>.EventHandleAsync(async () =>
                await _refreshTokenService.RefreshToken(refreshToken!, GetIpAddress()));

            if (result.IsSuccess && !string.IsNullOrEmpty(result.Data?.RefreshToken))
            {
                AttachAuthCookies(result.Data!);
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] string bodyToken)
        {
            var token = bodyToken ?? Request.Cookies[CookieNames.RefreshTokenCookie];

            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _refreshTokenService.RevokeToken(token!, GetIpAddress()));

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies[CookieNames.RefreshTokenCookie];
            await _refreshTokenService.RevokeToken(refreshToken!, GetIpAddress());

            Response.Cookies.Delete(CookieNames.RefreshTokenCookie);
            Response.Cookies.Delete(CookieNames.AccessToken);
            return Ok();
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