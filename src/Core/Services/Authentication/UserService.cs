using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DTO;
using Core.Entities.Options;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MTS.Common.Constants;
using MTS.Core.DTO;
using MTS.Core.Entities;
using MTS.Core.Interfaces.Repositories;
using MTS.Core.Interfaces.Services;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;
using MTS.Core.Services.Guide;

namespace MTS.Core.Services.Authentication
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly SignInManager<ApplicationUserModel> _signInManager;
        private readonly IJwtAuthentication _jwtAuthentication;
        private readonly GuideService _guide;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEntryRepository _entryRepository;
        private readonly JWTOptions _jwtOptions;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ICurrentUserService _currentUser;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UserService(UserManager<ApplicationUserModel> userManager,
            SignInManager<ApplicationUserModel> signInManager,
            ICurrentUserService currentUser,
            IJwtAuthentication jwtAuthentication,
            ICategoryRepository categoryRepository,
            IEntryRepository entryRepository,
            GuideService guide,
            IOptions<JWTOptions> jwtOptions,
            IRefreshTokenService refreshTokenService,
            IServiceScopeFactory serviceScopeFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtAuthentication = jwtAuthentication;
            _currentUser = currentUser;
            _categoryRepository = categoryRepository;
            _entryRepository = entryRepository;
            _guide = guide;
            _jwtOptions = jwtOptions.Value;
            _refreshTokenService = refreshTokenService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<UserInfoDto> GetUserInfoAsync()
        {
            var user = await _userManager.Users
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .FirstOrDefaultAsync(en => en.Id == _currentUser.UserId);

            if (user is null)
                throw new HandledException("Couldn't find this user");

            var categoriesCount = await _categoryRepository.GetAllAsync().CountAsync();
            var entriesCount = await _entryRepository.GetAllAsync().CountAsync();
            var accountSize = await _entryRepository.GetAllAsync().SumAsync(e => e.Size);

            UserInfoDto userInfo = new()
            {
                Username = user.UserName,
                AvatarUrl = user.AvatarUrl,
                AccountCreatedDate = user.AccountCreatedDate,
                AccountSize = accountSize,
                CategoriesCount = categoriesCount,
                EntriesCount = entriesCount,
                Roles = user.UserRoles.Select(e => e.Role.Name).ToArray()!,
                Email = user.Email
            };

            return userInfo;
        }

        public Task<List<ManagementUserDto>> GetManagementUsers()
        {
            return _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(e => e.Role)
                .OrderBy(u => u.NormalizedUserName)
                .Select(user => new ManagementUserDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.UserName,
                    AvatarUrl = user.AvatarUrl,
                    AccountCreatedDate = user.AccountCreatedDate,
                    Roles = user.UserRoles!.Select(ur => ur.Role.Name).ToArray()!,
                    AccountSize = user.Entries!.Sum(e => e.Size)
                })
                .ToListAsync();
        }

        public async Task<bool> ChangeUserAvatarAsync(string avatarUrl)
        {
            var user = await _userManager.FindByIdAsync(_currentUser?.UserId!);
            if (user is null)
                throw new HandledException("Couldn't find this user");

            if (user.AvatarUrl == avatarUrl)
                return true;

            user.AvatarUrl = avatarUrl ?? "";

            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ChangeUsernameAsync(string username, string password)
        {
            var user = await _userManager.FindByIdAsync(_currentUser?.UserId!);
            if (user is null)
                throw new HandledException("Couldn't find this user");

            if (string.IsNullOrEmpty(username))
                throw new HandledException("Username cannot be empty");

            if (user.UserName == username)
                return true;

            var passwordVerification = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordVerification)
                throw new HandledException("Wrong password");

            var duplicateUser = await _userManager.FindByNameAsync(username);
            if (duplicateUser is not null)
                throw new HandledException("This username is already taken");

            await _userManager.SetUserNameAsync(user, username);

            return true;
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
                throw new HandledException("New and old password can't be empty");

            var user = await _userManager.FindByIdAsync(_currentUser?.UserId!);
            if (user is null)
                throw new HandledException("Couldn't find this user");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
                throw new HandledException("Couldn't change password, the current password is incorrect");

            return true;
        }

        public async Task<IdentityResult> CreateUser(RegisterDto registerUser)
        {
            if (registerUser is null)
                throw new HandledException("Register User model cannot be empty");

            var user = new ApplicationUserModel()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName,
                Email = registerUser?.Email,
                AccountCreatedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerUser!.Password!);

            if (!result.Succeeded)
                throw new HandledExceptionList(result.Errors.Select(errors => errors.Description).ToList());

            // seed data
            await _userManager.AddToRoleAsync(user, Role.USER);
            await SeedGuide(user);

            return result;
        }

        public async Task<VerifiedUserDto> LoginUser(LoginUserDto userDto, string IpAddress)
        {
            if (userDto is null && string.IsNullOrEmpty(IpAddress))
                throw new HandledException("User model and Ip address cannot be empty");

            var user = await _userManager.Users.Where(u => u.UserName == userDto!.Username)
                                                .Include(e => e.UserRoles)
                                                .ThenInclude(e => e.Role)
                                                .Include(e => e.RefreshTokens) // consider .Take(n)
                                                .FirstOrDefaultAsync();

            return await HandleLogin(user!, userDto!.Password!, IpAddress);
        }

        public async Task<bool> ChangeEmailAsync(string email, string password)
        {
            var user = await _userManager.FindByIdAsync(_currentUser?.UserId!);
            if (user is null)
                throw new HandledException("Couldn't find this user");

            if (string.IsNullOrEmpty(email))
                throw new HandledException("Email cannot be empty");

            if (user.Email == email)
                return true;

            var passwordVerification = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordVerification)
                throw new HandledException("Wrong password");

            var duplicateUser = await _userManager.FindByEmailAsync(email);
            if (duplicateUser is not null)
                throw new HandledException("This email is already taken");

            var result = await _userManager.ChangeEmailAsync(user, email, "");

            if (!result.Succeeded)
                throw new HandledExceptionList(result.Errors.Select(errors => errors.Description).ToList());

            return result.Succeeded;
        }

        private async Task<VerifiedUserDto> HandleLogin(ApplicationUserModel user, string password, string ipAddress)
        {
            if (user is null)
                throw new HandledException("Couldn't log in, check your login or password"); // Couldn't find user

            // Validate credentials 
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new HandledException("Couldn't log in, check your login or password");

            var refreshToken = _refreshTokenService.CreateRefreshToken(ipAddress);
            user.RefreshTokens ??= new();
            user.RefreshTokens?.Add(refreshToken);

            var roles = user.UserRoles.Select(e => e.Role.Name).ToList();
            var token = _jwtAuthentication.GenerateJsonWebToken(user!, roles!);

            _refreshTokenService.RemoveOldRefreshTokens(user);

            // save removal of old refresh tokens
            await _userManager.UpdateAsync(user);

            // fire and forget log login
            LogLoginAsync(user, ipAddress);

            return new VerifiedUserDto()
            {
                Username = user!.UserName,
                Roles = roles?.ToArray()!,
                RefreshToken = refreshToken!.Token,
                RefreshTokenExpiration = refreshToken!.Expires,
                Token = token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpireTime),
                AvatarUrl = user.AvatarUrl,
                Email = user.Email
            };
        }

        private void LogLoginAsync(ApplicationUserModel user, string ipAddress)
        {
            _ = Task.Run(async () =>
            {
                await using var scope = _serviceScopeFactory.CreateAsyncScope();
                var loginLogService = scope.ServiceProvider.GetRequiredService<ILoginLogService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<object>>();

                try
                {
                    await loginLogService.AddLoginLogAsync(user, ipAddress);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to log a login");
                }
            });
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