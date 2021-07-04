using System.Threading.Tasks;
using Infrastructure.Models;

namespace App.Authentication
{
    public interface IJwtAuthentication
    {
         Task<string> GenerateJsonWebToken(ApplicationUser user);
    }
}