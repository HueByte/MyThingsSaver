using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using App.Authentication;
using App.Guide;
using Common.Types;
using Core.Models;
using Core.RepositoriesInterfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
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
            var databaseType = _configuration.GetValue<string>("Database:Type").ToLower();

            if (string.IsNullOrEmpty(databaseType))
                throw new ArgumentException("Database type cannot be empty");

            if (databaseType == DatabaseType.MYSQL)
            {
                if (isProduction)
                    _services.AddDbContextMysqlProduction(_configuration);
                else
                    _services.AddDbContextMysqlDebug(_configuration);
            }
            else if (databaseType == DatabaseType.SQLITE)
            {
                _services.AddDbContextSqlite(_configuration);
            }
            else
                throw new Exception("Invalid database, please provide correct value in appsettings.json");
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
                    ValidateIssuerSigningKey = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
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

            // guide 
            GuideService _guide = new();
            _services.AddSingleton(_guide);
        }

        public void ConfigureCors(string[] origins) => _services.AddCors(o => o.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins(origins)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        }));

        public void ConfigureForwardedHeaders()
        {
            var type = _configuration.GetValue<string>("Network:Type").ToLower();

            if (type == NetworkType.isNginx)
                _services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                });
        }
    }
}