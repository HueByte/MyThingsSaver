using Microsoft.AspNetCore.Identity;
using MTS.Core.DTO;

namespace MTS.Core.Interfaces.Services;

public interface IUserService
{
    Task<IdentityResult> CreateUser(RegisterDto registerUser);
    Task<VerifiedUserDto> LoginUser(LoginUserDto userDTO, string IpAddress);
    // Task ChangePasswordAsync(ChangePasswordDto user);
}