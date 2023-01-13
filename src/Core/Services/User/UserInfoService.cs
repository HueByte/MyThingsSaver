using Core.DTO;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MTS.Core.Entities;
using MTS.Core.Interfaces.Repositories;
using MTS.Core.Models;
using MTS.Core.Services.CurrentUser;
using Z.EntityFramework.Plus;

namespace Core.Services.User
{
    public class UserInfoService : IUserInfoService
    {
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly ICurrentUserService _currentUser;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEntryRepository _entryRepository;
        public UserInfoService(UserManager<ApplicationUserModel> userManager, ICurrentUserService currentUser, ICategoryRepository categoryRepository, IEntryRepository entryRepository)
        {
            _userManager = userManager;
            _currentUser = currentUser;
            _categoryRepository = categoryRepository;
            _entryRepository = entryRepository;
        }

        public async Task<UserInfoDto> GetUserInfoAsync()
        {
            var user = await _userManager.FindByIdAsync(_currentUser?.UserId!);

            if (user is null)
                throw new EndpointException("Couldn't find this user");

            var categoriesCount = await (await _categoryRepository.GetAllAsync()).CountAsync();
            var entriesCount = await (await _entryRepository.GetAllAsync()).CountAsync();
            var roles = await _userManager.GetRolesAsync(user);

            UserInfoDto userInfo = new()
            {
                Username = user.UserName,
                AvatarUrl = user.AvatarUrl,
                AccountCreatedDate = user.AccountCreatedDate,
                CategoriesCount = categoriesCount,
                EntriesCount = entriesCount,
                Roles = roles.ToArray()
            };

            return userInfo;
        }

        public async Task<bool> ChangeUserAvatarAsync(string avatarUrl)
        {
            var user = await _userManager.FindByIdAsync(_currentUser?.UserId!);
            if (user is null)
                throw new EndpointException("Couldn't find this user");

            if (user.AvatarUrl == avatarUrl)
                return true;

            user.AvatarUrl = avatarUrl ?? "";

            await _userManager.UpdateAsync(user);

            return true;
        }
    }
}