using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Constants;
using Core.DTO;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Services.Guide;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Authentication
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly SignInManager<ApplicationUserModel> _signInManager;
        private readonly IJwtAuthentication _jwtAuthentication;
        private readonly GuideService _guide;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEntryRepository _entryRepository;
        private readonly AppSettingsRoot _settings;
        private readonly IRefreshTokenService _refreshTokenService;
        public UserService(UserManager<ApplicationUserModel> userManager,
                           SignInManager<ApplicationUserModel> signInManager,
                           IJwtAuthentication jwtAuthentication,
                           ICategoryRepository categoryRepository,
                           IEntryRepository entryRepository,
                           GuideService guide,
                           AppSettingsRoot settings,
                           IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtAuthentication = jwtAuthentication;
            _categoryRepository = categoryRepository;
            _entryRepository = entryRepository;
            _guide = guide;
            _settings = settings;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<IdentityResult> CreateUser(RegisterDto registerUser)
        {
            if (registerUser is null)
                throw new ArgumentException("RegisterUser model cannot be null");

            var user = new ApplicationUserModel()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName,
                Email = registerUser?.Email
            };

            var result = await _userManager.CreateAsync(user, registerUser?.Password);

            if (!result.Succeeded)
                throw new EndpointExceptionList(result.Errors.Select(errors => errors.Description).ToList());

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
                throw new Exception("New and old password can't be empty");

            var user = await _userManager.FindByNameAsync(userDTO.UserName);
            if (user is null)
                throw new Exception("Couldn't find user");

            var result = await _userManager.ChangePasswordAsync(user, userDTO.OldPassword, userDTO.NewPassword);

            if (!result.Succeeded)
                throw new EndpointExceptionList(result.Errors.Select(errors => errors.Description).ToList());
        }


        public async Task<VerifiedUserDto> LoginUser(LoginUserDto userDto, string IpAddress)
        {
            if (userDto is null && string.IsNullOrEmpty(IpAddress))
                throw new ArgumentException("User model and Ip address cannot be empty");

            var user = await _userManager.Users.Where(u => u.UserName == userDto!.Username)
                                               .Include(e => e.RefreshTokens) // consider .Take(n)
                                               .FirstOrDefaultAsync();

            return await HandleLogin(user!, userDto!.Password, IpAddress);
        }

        private async Task<VerifiedUserDto> HandleLogin(ApplicationUserModel user, string password, string ipAddress)
        {
            if (user is null)
                throw new EndpointException("Couldn't log in, check your login or password"); // Couldn't find user

            // Validate credentials 
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new EndpointException("Couldn't log in, check your login or password");

            var refreshToken = _refreshTokenService.CreateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);
            var roles = await _userManager.GetRolesAsync(user!);
            var token = _jwtAuthentication.GenerateJsonWebToken(user!, roles);

            await _refreshTokenService.RemoveOldRefreshTokens(user);

            // save removal of old refresh tokens
            await _userManager.UpdateAsync(user);

            return new VerifiedUserDto()
            {
                Username = user!.UserName,
                Roles = roles.ToArray(),
                RefreshToken = refreshToken!.Token,
                RefreshTokenExpiration = refreshToken!.Expires,
                Token = token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_settings.JWT.AccessTokenExpireTime)
            };
        }

        private async Task SeedGuide(ApplicationUserModel user)
        {
            var categoryId = Guid.NewGuid().ToString();
            CategoryModel guideCategory = new()
            {
                Entries = null,
                Id = categoryId,
                CreatedDate = DateTime.UtcNow,
                Name = "Guide",
                UserId = user.Id,
                Path = $"{user.Id}/{categoryId}",
                Level = 0
            };

            EntryModel welcome = new()
            {
                Name = "Welcome",
                CategoryId = categoryId,
                Content = _guide.WELCOME,
                Size = _guide.WELCOME_SIZE,
                CreatedOn = DateTime.UtcNow,
                LastUpdatedOn = DateTime.UtcNow,
                Image = null,
                UserId = user.Id,
                Id = Guid.NewGuid().ToString()
            };

            EntryModel guide = new()
            {
                Name = "Guide",
                CategoryId = categoryId,
                Content = _guide.GUIDE,
                Size = _guide.GUIDE_SIZE,
                CreatedOn = DateTime.UtcNow,
                LastUpdatedOn = DateTime.UtcNow,
                Image = null,
                UserId = user.Id,
                Id = Guid.NewGuid().ToString()
            };

            await _categoryRepository.AddAsync(guideCategory);
            await _entryRepository.AddRangeAsync(new EntryModel[] { welcome, guide });
            await _categoryRepository.SaveChangesAsync();
        }
    }
}