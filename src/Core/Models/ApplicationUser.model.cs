using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using MTS.Core.Abstraction;

namespace MTS.Core.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        public ICollection<CategoryModel> Categories { get; set; }
        public ICollection<EntryModel> Entries { get; set; }
        public virtual List<RefreshTokenModel> RefreshTokens { get; set; }
    }
}