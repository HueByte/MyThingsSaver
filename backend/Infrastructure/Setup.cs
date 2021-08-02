using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    // navigate to root folder of infrastructure
    // run migration with: dotnet ef migrations add migration --startup-project ../App/
    // update database with: dotnet ef database update --startup-project ../App
    public static class Setup
    {
        // For some reason this shittery must look like that for non-problematic migrations with MySQL
        public static void AddDbContextMysqlProduction(this IServiceCollection services, IConfiguration config) =>
            services.AddDbContext<AppDbContext>(
                                options => options.UseMySql(config.GetConnectionString("DatabaseConnectionString"),
                                ServerVersion.AutoDetect(config.GetConnectionString("DatabaseConnectionString")),
                                x => x.MigrationsAssembly("Infrastructure"))

            );

        // // For some reason this shittery must look like that for non-problematic migrations with MySQL
        public static void AddDbContextMysqlDebug(this IServiceCollection services, IConfiguration config) =>
            services.AddDbContext<AppDbContext>(
                                options => options.UseMySql(config.GetConnectionString("DatabaseConnectionString"),
                                ServerVersion.AutoDetect(config.GetConnectionString("DatabaseConnectionString")),
                                x => x.MigrationsAssembly("Infrastructure"))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );

        public static void AddDbContextSqlite(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlite(config.GetConnectionString("SQLiteConnectionString"), x => x.MigrationsAssembly("Infrastructure"))
            );
        }
    }
}