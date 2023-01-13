using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MTS.Core.Abstraction;

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
    }
}