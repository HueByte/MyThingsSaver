using System;
using System.Threading.Tasks;
using App.Authentication;
using App.Extensions;
using Common.Events;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class AuthenticateController : BaseApiController
    {
        private readonly IUserService _userService;
        public AuthenticateController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO registerUser)
        {
            var result = await ApiEventHandler.EventHandleAsync(async () =>
                await _userService.CreateUser(registerUser));

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("LoginEmail")]
        public async Task<IActionResult> LoginEmail([FromBody] LoginEmailDTO user)
        {
            var result = await ApiEventHandler<VerifiedUser>.EventHandleAsync(async () =>
                await _userService.LoginUserWithEmail(user));

            if (result.IsSuccess)
            {
                SetRefreshTokenCookie(result.Data.RefreshToken);
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("LoginUsername")]
        public async Task<IActionResult> LoginUsername([FromBody] LoginUserDTO user)
        {
            var result = await ApiEventHandler<VerifiedUser>.EventHandleAsync(async () =>
                await _userService.LoginUserWithUsername(user));

            if (result.IsSuccess)
            {
                SetRefreshTokenCookie(result.Data.RefreshToken);
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO user)
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
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await ApiEventHandler<VerifiedUser>.EventHandleAsync(async () =>
                await _userService.RefreshTokenAsync(refreshToken));

            if (result.IsSuccess && !string.IsNullOrEmpty(result.Data.RefreshToken))
            {
                SetRefreshTokenCookie(result.Data.RefreshToken);
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] string bodyToken)
        {
            var token = bodyToken ?? Request.Cookies["refreshToken"];

            var result = await ApiEventHandler<string>.EventHandleAsync(async () =>
            {
                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token is required");

                var response = await _userService.RevokeTokenAsync(token);

                if (!response)
                    throw new Exception("Token not found");

                return "Token Revoked";
            });

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}