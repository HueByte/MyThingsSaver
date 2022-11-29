using System.Collections.Generic;
using Core.Abstraction;
using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        public ICollection<CategoryModel> Categories { get; set; }
        public ICollection<EntryModel> Entries { get; set; }
        public virtual List<RefreshTokenModel> RefreshTokens { get; set; }
    }
}