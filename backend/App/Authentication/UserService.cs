using App.Guide;
using Common.Constants;
using Common.Events;
using Core.DTO;
using Core.Entities;
using Core.Models;
using Infrastructure;
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
        private readonly GuideService _guide;
        private readonly AppSettingsRoot _settings;
        public UserService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IJwtAuthentication jwtAuthentication,
                           AppDbContext context,
                           GuideService guide,
                           AppSettingsRoot settings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtAuthentication = jwtAuthentication;
            _context = context;
            _guide = guide;
            _settings = settings;
        }

        public async Task<IdentityResult> CreateUser(RegisterDto registerUser)
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

            // seed data
            await _userManager.AddToRoleAsync(user, Role.USER);
            await SeedGuide(user);

            return result;
        }

        public async Task ChangePasswordAsync(ChangePasswordDto userDTO)
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


        public async Task<VerifiedUser> LoginUser(LoginUserDto userDto)
        {
            if (userDto == null)
                throw new ArgumentException("User model cannot be null");

            var user = await _userManager.Users.Where(u => u.UserName == userDto.Username)
                                               .Include(e => e.RefreshTokens)
                                               .FirstOrDefaultAsync();

            return await HandleLogin(user, userDto.Password);
        }

        private async Task<VerifiedUser> HandleLogin(ApplicationUser user, string password)
        {
            if (user == null)
                throw new Exception("Couldn't log in, check your login or password"); // Couldn't find user

            // Validate credentials 
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new Exception("Couldn't log in, check your login or password");

            // Get or Create refresh token for JWT
            RefreshToken activeRefreshToken;
            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                activeRefreshToken = user.RefreshTokens?.FirstOrDefault(a => a.IsActive == true);
            }
            else
            {
                activeRefreshToken = _jwtAuthentication.CreateRefreshToken();
                user.RefreshTokens.Add(activeRefreshToken);
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT
            var token = _jwtAuthentication.GenerateJsonWebToken(user, roles);

            return new VerifiedUser()
            {
                Username = user.UserName,
                Roles = roles.ToArray(),
                RefreshToken = activeRefreshToken?.Token,
                RefreshTokenExpiration = DateTime.Now.AddMinutes(_settings.JWT.RefreshTokenExpireTime),
                Token = _jwtAuthentication.GenerateJsonWebToken(user, roles),
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_settings.JWT.AccessTokenExpireTime)
            };
        }

        public async Task<VerifiedUser> RefreshTokenAsync(string token)
        {
            var user = await _userManager.Users.Include(e => e.RefreshTokens)
                                               .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
                throw new Exception("Token did not match any user");

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
                Username = user.UserName,
                Roles = roles.ToArray(),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = DateTime.Now.AddMinutes(_settings.JWT.RefreshTokenExpireTime),
                Token = _jwtAuthentication.GenerateJsonWebToken(user, roles),
                AccessTokenExpiration = DateTime.Now.AddMinutes(_settings.JWT.AccessTokenExpireTime)
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

        private async Task SeedGuide(ApplicationUser user)
        {
            var categoryId = Guid.NewGuid().ToString();
            Category guideCategory = new()
            {
                CategoryEntries = null,
                CategoryId = categoryId,
                DateCreated = DateTime.UtcNow,
                Name = "Guide",
                OwnerId = user.Id,
                Path = $"{user.Id}/{categoryId}",
                Level = 0
            };

            CategoryEntry welcome = new()
            {
                CategoryEntryName = "Welcome",
                CategoryId = categoryId,
                Content = _guide.WELCOME,
                Size = _guide.WELCOME_SIZE,
                CreatedOn = DateTime.UtcNow,
                LastUpdatedOn = DateTime.UtcNow,
                Image = null,
                OwnerId = user.Id,
                CategoryEntryId = Guid.NewGuid().ToString()
            };

            CategoryEntry guide = new()
            {
                CategoryEntryName = "Guide",
                CategoryId = categoryId,
                Content = _guide.GUIDE,
                Size = _guide.GUIDE_SIZE,
                CreatedOn = DateTime.UtcNow,
                LastUpdatedOn = DateTime.UtcNow,
                Image = null,
                OwnerId = user.Id,
                CategoryEntryId = Guid.NewGuid().ToString()
            };

            await _context.Categories.AddAsync(guideCategory);
            await _context.CategoriesEntries.AddRangeAsync(new CategoryEntry[] { welcome, guide });
            await _context.SaveChangesAsync();
        }
    }
}