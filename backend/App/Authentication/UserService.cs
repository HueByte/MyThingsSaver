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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtAuthentication _jwtAuthentication;
        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtAuthentication jwtAuthentication)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtAuthentication = jwtAuthentication;
        }

        public async Task<IdentityResult> CreateUser(RegisterDTO registerUser)
        {
            if (registerUser is null)
                throw new ArgumentException("RegisterUser model cannot be null");

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

        public async Task ChangePasswordAsync(ChangePasswordDTO userDTO)
        {
            if (userDTO is null)
                throw new ArgumentException("User model cannot be null");

            if (string.IsNullOrEmpty(userDTO.OldPassword) || string.IsNullOrEmpty(userDTO.NewPassword))
                throw new Exception("New nad old password can't be empty");

            var user = await _userManager.FindByNameAsync(userDTO.UserName);
            if (user is null)
                throw new Exception("Couldn't find user");

            var result = await _userManager.ChangePasswordAsync(user, userDTO.OldPassword, userDTO.NewPassword);

            if (!result.Succeeded)
                throw new ExceptionList(result.Errors.Select(errors => errors.Description).ToList());
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
                throw new Exception("Couldn't log in, check your login or password"); // Couldn't find user

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                throw new Exception("Couldn't log in, check your login or password");

            var roles = await _userManager.GetRolesAsync(user);

            return new VerifiedUser()
            {
                Token = await _jwtAuthentication.GenerateJsonWebToken(user, roles),
                ExpireDate = DateTime.Now.AddDays(3),
                TokenType = "Bearer",
                Roles = roles.ToArray(),
                Username = user.UserName
            };
        }
    }
}