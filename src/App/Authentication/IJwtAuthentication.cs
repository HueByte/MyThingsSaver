using Core.Models;

namespace App.Authentication
{
    public interface IJwtAuthentication
    {
        string GenerateJsonWebToken(ApplicationUserModel user, IList<string> roles);
    }
}