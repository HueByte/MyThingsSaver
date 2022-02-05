using System.Threading.Tasks;
using Core.DTO;
using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace App.Authentication
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(RegisterDTO registerUser);
        Task<VerifiedUser> LoginUser(LoginUserDTO userDTO);
        Task<VerifiedUser> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
        Task ChangePasswordAsync(ChangePasswordDTO user);
    }
}