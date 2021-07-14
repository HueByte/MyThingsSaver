using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public partial class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryEntry> CategoriesEntries { get; set; }
    }
}