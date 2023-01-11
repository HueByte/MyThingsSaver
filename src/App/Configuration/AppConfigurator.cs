using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MTS.Infrastructure;

namespace MTS.App.Configuration
{
    public static class AppConfigurator
    {
        public static async Task<WebApplication> MigrateAsync(this WebApplication webapp)
        {
            await using var scope = webapp.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<MTSContext>();

            await context.Database.MigrateAsync();

            return webapp;
        }

        public static async Task<WebApplication> SeedIdentity(this WebApplication webapp)
        {
            await using var scope = webapp.Services.CreateAsyncScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUserModel>>();
            string[] roles = { Role.USER, Role.ADMIN };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

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