using System.Threading.Tasks;
using Core.DTO;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces.Services;

public interface IUserService
{
    Task<IdentityResult> CreateUser(RegisterDto registerUser);
    Task<VerifiedUserDto> LoginUser(LoginUserDto userDTO, string IpAddress);
    Task ChangePasswordAsync(ChangePasswordDto user);
}