using Microsoft.AspNetCore.Identity;

namespace MTS.Core.Models
{
    public class RoleModel : IdentityRole
    {
        public virtual ICollection<UserRoleModel>? UserRoles { get; set; }
    }
}