using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Events;
using Common.Types;
using Core.Entities;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Authentication
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtAuthentication _jwtAuthentication;
        private readonly AppDbContext _context;
        public UserService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IJwtAuthentication jwtAuthentication,
                           AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtAuthentication = jwtAuthentication;
            _context = context;

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

            // var user = await _userManager.FindByEmailAsync(userDTO.Email);
            var user = await _userManager.Users.Where(u => u.Email == userDTO.Email)
                                   .Include(e => e.RefreshTokens)
                                   .FirstOrDefaultAsync();

            return await HandleLogin(user, userDTO.Password);
        }

        public async Task<VerifiedUser> LoginUserWithUsername(LoginUserDTO userDTO)
        {
            if (userDTO == null)
                throw new ArgumentException("User model cannot be null");

            // var user = await _userManager.FindByNameAsync(userDTO.Username);
            var user = await _userManager.Users.Where(u => u.UserName == userDTO.Username)
                                               .Include(e => e.RefreshTokens)
                                               .FirstOrDefaultAsync();

            return await HandleLogin(user, userDTO.Password);
        }

        private async Task<VerifiedUser> HandleLogin(ApplicationUser user, string password)
        {
            if (user == null)
                throw new Exception("Couldn't log in, check your login or password"); // Couldn't find user

            // Validate credentials 
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new Exception("Couldn't log in, check your login or password");

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Get or Create refresh token for JWT
            RefreshToken activeRefreshToken;
            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
            }
            else
            {
                activeRefreshToken = _jwtAuthentication.CreateRefreshToken();
                user.RefreshTokens.Add(activeRefreshToken);
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            return new VerifiedUser()
            {
                Token = _jwtAuthentication.GenerateJsonWebToken(user, roles),
                ExpireDate = DateTime.Now.AddDays(3),
                RefreshToken = activeRefreshToken.Token,
                RefreshTokenExpiration = activeRefreshToken.Expires,
                TokenType = "Bearer",
                Roles = roles.ToArray(),
                Username = user.UserName
            };
        }

        public async Task<VerifiedUser> RefreshTokenAsync(string token)
        {
            var user = _userManager.Users.Include(e => e.RefreshTokens)
                                         .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
                throw new Exception("Token did not match any users");

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
                throw new Exception("Token not active");

            // revoke current refresh token
            refreshToken.Revoked = DateTime.UtcNow;

            // generate new refresh token and save to database
            var newRefreshToken = _jwtAuthentication.CreateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            await _context.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);

            return new VerifiedUser()
            {
                Token = _jwtAuthentication.GenerateJsonWebToken(user, roles),
                ExpireDate = DateTime.Now.AddDays(3),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.Expires,
                TokenType = "Bearer",
                Roles = roles.ToArray(),
                Username = user.UserName
            };
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}