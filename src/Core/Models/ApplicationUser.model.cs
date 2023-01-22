using Microsoft.AspNetCore.Identity;

namespace MTS.Core.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        public string? AvatarUrl { get; set; }
        public DateTime AccountCreatedDate { get; set; }
        public ICollection<CategoryModel>? Categories { get; set; }
        public ICollection<EntryModel>? Entries { get; set; }
        public ICollection<LoginLogModel>? LoginLogs { get; set; }
        public virtual List<RefreshTokenModel>? RefreshTokens { get; set; }
        public virtual ICollection<UserRoleModel> UserRoles { get; set; } = default!;
    }
}