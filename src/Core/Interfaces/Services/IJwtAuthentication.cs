using System.Collections.Generic;
using MTS.Core.Models;

namespace MTS.Core.Interfaces.Services;

public interface IJwtAuthentication
{
    string GenerateJsonWebToken(ApplicationUserModel user, IList<string> roles);
}