using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MTS.Core.Models
{
    public class UserRoleModel : IdentityUserRole<string>
    {
        public virtual ApplicationUserModel User { get; set; } = default!;
        public virtual RoleModel Role { get; set; } = default!;
    }
}