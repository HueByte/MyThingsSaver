using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Authentication;
using Core.Models;
using Core.RepositoriesInterfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace App.Configuration
{
    public class ModuleConfiguration
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;
        public ModuleConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public void ConfigureDatabase(bool isProduction)
        {
            var doesUseMySql = _configuration.GetValue<bool>("Database:DoesUseMySQL");
            var doesUseSqlite = _configuration.GetValue<bool>("Database:DoesUseSQLite");
            if ((doesUseSqlite && doesUseMySql) || (!doesUseSqlite && !doesUseMySql))
                throw new Exception("Please use one database, change your appsettings.json and set one of them to true");

            if (doesUseMySql)
            {
                if (isProduction)
                    _services.AddDbContextMysqlProduction(_configuration);
                else
                    _services.AddDbContextMysqlDebug(_configuration);
            }
            if (doesUseSqlite)
                _services.AddDbContextSqlite(_configuration);
        }

        public void ConfigureSecurity()
        {
            _services.AddIdentity<ApplicationUser, IdentityRole>()
                     .AddEntityFrameworkStores<AppDbContext>()
                     .AddDefaultTokenProviders();

            _services.Configure<IdentityOptions>(options =>
            {
                // password options
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // user settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            _services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                    ValidateIssuerSigningKey = true
                };
            });
        }

        // TODO: Compare performance between ASP.NET 5 static file serving and nginx
        public void ConfigureSpa()
        {
            // TODO: Fix locating files on linux/VM 
            // Localhost refused to connect
            // also see Startup.cs#95
            _services.AddSpaStaticFiles(config =>
            {
                config.RootPath = @"build";
            });
        }

        public void ConfigureServices()
        {
            _services.AddScoped<IJwtAuthentication, JwtAuthentication>();
            _services.AddScoped<IUserService, UserService>();
            _services.AddScoped<ICategoryRepository, CategoryRepository>();
            _services.AddScoped<ICategoryEntryRepository, CategoryEntryRepository>();
        }

        public void ConfigureCors(string[] origins) => _services.AddCors(o => o.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins(origins)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        }));
    }
}