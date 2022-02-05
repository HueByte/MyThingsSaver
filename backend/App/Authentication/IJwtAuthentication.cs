using Core.Models;

namespace App.Authentication
{
    public interface IJwtAuthentication
    {
        string GenerateJsonWebToken(ApplicationUser user, IList<string> roles);
        RefreshToken CreateRefreshToken();
    }
}