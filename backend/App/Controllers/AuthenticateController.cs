using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Authentication;
using App.Extensions;
using Common.ApiResonse;
using Common.Types;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Models;

namespace App.Controllers
{
    public class AuthenticateController : BaseApiController
    {
        private UserManager<ApplicationUser> _userManager;
        private IJwtAuthentication _jwtAuthentication;
        private IUserService _userService;
        public AuthenticateController(UserManager<ApplicationUser> userManager, IJwtAuthentication jwtAuthentication, IUserService userService)
        {
            _userManager = userManager;
            _jwtAuthentication = jwtAuthentication;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<BaseApiResponse<string>> RegisterUser([FromBody] RegisterDTO registerUser)
        {
            IdentityResult result;
            try
            {
                result = await _userService.CreateUser(registerUser);
            }
            catch (Exception e)
            {
                return new BaseApiResponse<string>()
                {
                    Data = null,
                    Errors = new List<string>() { e.Message },
                    IsSuccess = false
                };
            }

            if (result.Succeeded)
            {
                return new BaseApiResponse<string>()
                {
                    Data = null,
                    Errors = null,
                    IsSuccess = true
                };
            }
            else
            {
                return new BaseApiResponse<string>()
                {
                    Data = null,
                    Errors = result.Errors.Select(errors => errors.Description).ToList(),
                    IsSuccess = false
                };
            }
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