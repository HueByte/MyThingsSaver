using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Events;
using Common.Types;
using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace App.Authentication
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtAuthentication _jwtAuthentication;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IJwtAuthentication jwtAuthentication)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtAuthentication = jwtAuthentication;
        }

        public async Task<IdentityResult> CreateUser(RegisterDTO registerUser)
        {
            if (registerUser == null)
                throw new System.Exception("Register model cannot be null");

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName,
                Email = registerUser?.Email
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded)
                throw new ExceptionList(result.Errors.Select(errors => errors.Description).ToList());

            await _userManager.AddToRoleAsync(user, Role.USER);
            return result;
        }

        public async Task<VerifiedUser> LoginUserWithEmail(LoginEmailDTO userDTO)
        {
            if (userDTO == null)
                throw new ArgumentException("User model cannot be null");

            var user = await _userManager.FindByEmailAsync(userDTO.Email);

            return await HandleLogin(user, userDTO.Password);
        }

        public async Task<VerifiedUser> LoginUserWithUsername(LoginUserDTO userDTO)
        {
            if (userDTO == null)
                throw new ArgumentException("User model cannot be null");

            var user = await _userManager.FindByNameAsync(userDTO.Username);

            return await HandleLogin(user, userDTO.Password);
        }

        private async Task<VerifiedUser> HandleLogin(ApplicationUser user, string password)
        {
            if (user == null)
                throw new Exception("Couldn't find user");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                throw new Exception("Couldn't log in, check your login or password");

            return new VerifiedUser()
            {
                Token = await _jwtAuthentication.GenerateJsonWebToken(user),
                ExpireDate = DateTime.Now.AddDays(7),
                TokenType = "Bearer",
                Username = user.UserName
            };

        }
    }
}