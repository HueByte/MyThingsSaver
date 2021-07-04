using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.Authentication
{
    public interface IUserService
    {
         Task<IdentityResult> CreateUser(RegisterDTO registerUser);
    }
}