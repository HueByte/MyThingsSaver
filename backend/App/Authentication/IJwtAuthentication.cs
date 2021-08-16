using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace App.Authentication
{
    public interface IJwtAuthentication
    {
        Task<string> GenerateJsonWebToken(ApplicationUser user, IList<string> roles);
    }
}