using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MTS.Infrastructure
{
    // Manual migration
    // navigate to root folder of infrastructure
    // run migration with: dotnet ef migrations add migration --startup-project ../App/
    // update database with: dotnet ef database update --startup-project ../App
    public static class Setup
    {
        public static void AddDbContextMysqlProduction(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<MTSContext>(
                                options => options.UseMySql(connectionString,
                                ServerVersion.AutoDetect(connectionString),
                                x => x.MigrationsAssembly(typeof(Setup).Assembly.GetName().Name))
            );

        public static void AddDbContextMysqlDebug(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<MTSContext>(
                                options => options.UseMySql(connectionString,
                                ServerVersion.AutoDetect(connectionString),
                                x => x.MigrationsAssembly(typeof(Setup).Assembly.GetName().Name))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );

        public static void AddDbContextSqlite(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MTSContext>(
                options => options.UseSqlite(connectionString, x => x.MigrationsAssembly(typeof(Setup).Assembly.GetName().Name))
            );
        }
    }
}