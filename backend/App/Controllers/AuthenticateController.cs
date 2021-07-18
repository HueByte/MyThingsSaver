using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using App.Authentication;
using App.Extensions;
using Common.ApiResonse;
using Common.Events;
using Common.Types;
using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class AuthenticateController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtAuthentication _jwtAuthentication;
        private readonly IUserService _userService;
        public AuthenticateController(UserManager<ApplicationUser> userManager, IJwtAuthentication jwtAuthentication, IUserService userService)
        {
            _userManager = userManager;
            _jwtAuthentication = jwtAuthentication;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDTO registerUser)
        {
            var result = await ApiEventHandler.EventHandleAsync(async () => await _userService.CreateUser(registerUser));

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("LoginEmail")]
        public async Task<IActionResult> LoginEmail([FromBody] LoginEmailDTO user)
        {
            VerifiedUser result;

            if (user == null)
                return BadRequest();

            try
            {
                result = await _userService.LoginUserWithEmail(user);
                return Ok(new BaseApiResponse<VerifiedUser>()
                {
                    Data = result,
                    Errors = null,
                    IsSuccess = true
                });
            }
            catch (Exception e)
            {
                return BadRequest(new BaseApiResponse<string>()
                {
                    Data = null,
                    Errors = new List<string>() { e.Message },
                    IsSuccess = false
                });
            }
        }

        [HttpPost("LoginUsername")]
        public async Task<IActionResult> LoginUsername([FromBody] LoginUserDTO user)
        {
            VerifiedUser result;

            if (user == null)
                return BadRequest();

            try
            {
                result = await _userService.LoginUserWithUsername(user);
                return Ok(new BaseApiResponse<VerifiedUser>()
                {
                    Data = result,
                    Errors = null,
                    IsSuccess = true
                });
            }
            catch (Exception e)
            {
                return BadRequest(new BaseApiResponse<string>()
                {
                    Data = null,
                    Errors = new List<string>() { e.Message },
                    IsSuccess = false
                });
            }
        }
    }
}