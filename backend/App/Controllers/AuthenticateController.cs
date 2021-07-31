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
        private readonly IUserService _userService;
        public AuthenticateController(IUserService userService)
        {
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
            var result = await ApiEventHandler<VerifiedUser>.EventHandleAsync(async () => { return await _userService.LoginUserWithEmail(user); });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("LoginUsername")]
        public async Task<IActionResult> LoginUsername([FromBody] LoginUserDTO user)
        {
            var result = await ApiEventHandler<VerifiedUser>.EventHandleAsync(async () => { return await _userService.LoginUserWithUsername(user); });

            if (result.IsSuccess)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}