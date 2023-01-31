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
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleModel>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUserModel>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            string[] roles = { Role.USER, Role.ADMIN };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new RoleModel() { Name = role });
            }

            var adminUsers = config.GetSection("Admins")?.Get<string[]>();

            await CreateAdmins(adminUsers, userManager, logger);

            return webapp;
        }

        private static async Task CreateAdmins(string[]? adminUsers, UserManager<ApplicationUserModel> userManager, ILogger logger)
        {
            if (adminUsers is not null && adminUsers.Length > 0)
            {
                foreach (var user in adminUsers)
                {
                    var result = await AssignAdminRole(userManager, user);

                    if (result) logger.LogInformation("User {user} was assigned to admin role.", user);
                    else logger.LogInformation("User {user} failed to be assigned to admin role.", user);
                }
            }

            var admins = await userManager.GetUsersInRoleAsync(Role.ADMIN);
            adminUsers = adminUsers is null || adminUsers.Length == 0
                ? Array.Empty<string>()
                : adminUsers;

            foreach (var user in admins)
            {
                if (adminUsers.Contains(user.UserName)) continue;

                var result = await userManager.RemoveFromRoleAsync(user, Role.ADMIN);

                if (result.Succeeded) logger.LogInformation("User {username} was removed from admin role.", user.UserName);
                else logger.LogInformation("User {username} failed to be removed from admin role.", user.UserName);
            }
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