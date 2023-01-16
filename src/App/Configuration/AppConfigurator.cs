using Core.Entities.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MTS.Infrastructure;

namespace MTS.App.Configuration
{
    public static class AppConfigurator
    {
        public static WebApplication EnsureDatabaseFolder(this WebApplication webapp)
        {
            using var scope = webapp.Services.CreateScope();
            var dbOptions = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            var path = Path.Combine(AppContext.BaseDirectory, @"save");

            if (dbOptions?.Type == DatabaseType.SQLITE)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            return webapp;
        }

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
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
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

            var adminUsers = config.GetSection("Admins")?.Get<string[]>();

            if (adminUsers is null || adminUsers.Length <= 0) return webapp;

            foreach (var user in adminUsers)
            {
                var result = await AssignAdminRole(userManager, user);

                if (result) Console.WriteLine("User {0} was assigned to admin role.", user);
                else Console.WriteLine("User {0} failed to be assigned to admin role.", user);
            }

            var admins = await userManager.GetUsersInRoleAsync(Role.ADMIN);
            foreach (var user in admins)
            {
                if (adminUsers.Contains(user.UserName)) continue;

                var result = await userManager.RemoveFromRoleAsync(user, Role.ADMIN);

                if (result.Succeeded) Console.WriteLine("User {0} was removed from admin role.", user.UserName);
                else Console.WriteLine("User {0} failed to be removed from admin role.", user.UserName);
            }

            return webapp;
        }

        private static async Task<bool> AssignAdminRole(UserManager<ApplicationUserModel> userManager, string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user is null) return false;

            if (await userManager.IsInRoleAsync(user, Role.ADMIN)) return true;
            var result = await userManager.AddToRoleAsync(user, Role.ADMIN);

            return result.Succeeded;
        }
    }
}