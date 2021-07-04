using System;
using System.Threading.Tasks;
using Common.Types;
using Core.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace App.Authentication
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateUser(RegisterDTO registerUser)
        {
            if (registerUser == null)
                throw new System.Exception("Register model was null");

            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName,
                Email = registerUser?.Email
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, Role.USER);

            return result;
        }
    }
}