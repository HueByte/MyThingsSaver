using Common.Constants;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Configuration
{
    public static class AppConfigurator
    {
        public static async Task<WebApplication> Migrate(this WebApplication webapp)
        {
            var scope = webapp.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await context.Database.MigrateAsync();

            return webapp;
        }

        public static async Task<WebApplication> SeedIdentity(this WebApplication webapp)
        {
            var scope = webapp.Services.CreateAsyncScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUserModel>>();

            if (!await roleManager.RoleExistsAsync(Role.USER))
                await roleManager.CreateAsync(new IdentityRole(Role.USER));

            if (!await roleManager.RoleExistsAsync(Role.ADMIN))
                await roleManager.CreateAsync(new IdentityRole(Role.ADMIN));

            var admin = await userManager.FindByNameAsync("admin");
            if (admin is null)
            {
                admin = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    Email = "admin@heaven.org"
                };

                var result = await userManager.CreateAsync(admin, "Admin12");

                if (result.Succeeded) await userManager.AddToRoleAsync(admin, Role.ADMIN);
            }

            return webapp;
        }
    }
}