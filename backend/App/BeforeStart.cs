using System.Reflection.Metadata.Ecma335;
using Common.Constants;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App
{
    public class BeforeStart
    {
        private WebApplication _app;
        public BeforeStart(WebApplication app)
        {
            _app = app;
        }

        public BeforeStart PerformMigrations()
        {
            using var serviceScope = _app.Services?.GetService<IServiceScopeFactory>().CreateScope();

            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();

            return this;
        }

        public BeforeStart SeedIdentity()
        {
            using var serviceScope = _app.Services?.GetService<IServiceScopeFactory>().CreateScope();

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (!roleManager.RoleExistsAsync(Role.USER).GetAwaiter().GetResult())
                roleManager.CreateAsync(new IdentityRole(Role.USER)).GetAwaiter().GetResult();

            if (!roleManager.RoleExistsAsync(Role.ADMIN).GetAwaiter().GetResult())
                roleManager.CreateAsync(new IdentityRole(Role.ADMIN)).GetAwaiter().GetResult();

            var check = userManager.FindByNameAsync("admin").GetAwaiter().GetResult();
            if (check == null)
            {
                ApplicationUser admin = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    Email = "admin@xyz.com"
                };

                // This is intended for now as default password for admin which will be required to be changed
                var result = userManager.CreateAsync(admin, "Admin12").Result;
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, Role.ADMIN).Wait();
                }
            }

            return this;
        }

        public WebApplication Initialize() => _app;
    }
}