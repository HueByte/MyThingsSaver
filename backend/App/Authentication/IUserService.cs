using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace App.Authentication
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(RegisterDTO registerUser);
        Task ChangePasswordAsync(ChangePasswordDTO user);
        Task<VerifiedUser> LoginUserWithEmail(LoginEmailDTO userDTO);
        Task<VerifiedUser> LoginUserWithUsername(LoginUserDTO userDTO);
        Task<VerifiedUser> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}