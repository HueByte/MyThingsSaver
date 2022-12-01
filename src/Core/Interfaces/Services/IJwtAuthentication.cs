using System.Collections.Generic;
using Core.Models;

namespace Core.Interfaces.Services;

public interface IJwtAuthentication
{
    string GenerateJsonWebToken(ApplicationUserModel user, IList<string> roles);
}