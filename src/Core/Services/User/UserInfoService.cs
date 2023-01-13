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

        public async Task<bool> ChangeUsernameAsync(string username)
        {
            var user = await _userManager.FindByIdAsync(_currentUser?.UserId!);
            if (user is null)
                throw new EndpointException("Couldn't find this user");

            if (string.IsNullOrEmpty(username))
                throw new EndpointException("Username cannot be empty");

            if (user.UserName == username)
                return true;

            var duplicateUser = await _userManager.FindByNameAsync(username);
            if (duplicateUser is not null)
                throw new EndpointException("This username is already taken");

            await _userManager.SetUserNameAsync(user, username);

            return true;
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(_currentUser?.UserId!);
            if (user is null)
                throw new EndpointException("Couldn't find this user");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!result.Succeeded)
                throw new EndpointException("Couldn't change password, the current password is incorrect");

            return true;
        }
    }
}