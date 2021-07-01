using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Infrastructure
{
    public static class Setup
    {
        public static void AddDbContextProduction(this IServiceCollection services, string connectionString)
        {
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContext<AppDbContext>(
                options => options.UseMySql(serverVersion)
            );
        }

        public static void AddDbContextDebug(this IServiceCollection services, string connectionString) 
        {
            var serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContext<AppDbContext>(
                options => options.UseMySql(serverVersion)
                                  .EnableSensitiveDataLogging()
                                  .EnableDetailedErrors()
            );
        }
    }
}