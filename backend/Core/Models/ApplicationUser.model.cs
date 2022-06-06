using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Category> Categories { get; set; }
        public ICollection<CategoryEntry> Entries { get; set; }
        public virtual List<RefreshToken> RefreshTokens { get; set; }
    }
}