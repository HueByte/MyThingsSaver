using System.Text;
using System.Text.Json.Serialization;
using App.Authentication;
using App.Guide;
using Common.Types;
using Core.Entities;
using Core.Models;
using Core.RepositoriesInterfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace App.Configuration
{
    public class ModuleConfiguration
    {
        private readonly IServiceCollection _services;
        private readonly AppSettingsRoot _configuration;
        public ModuleConfiguration(IServiceCollection services, AppSettingsRoot configuration)
        {
            _services = services ?? new ServiceCollection();
            _configuration = configuration;
        }

        public ModuleConfiguration AddAppSettings()
        {
            _services.AddSingleton(_configuration);

            return this;
        }

        public ModuleConfiguration ConfigureDatabase(bool isProduction)
        {
            // var databaseType = _configuration.GetValue<string>("Database:Type").ToLower();
            var databaseType = _configuration.Database.Type.ToLower();

            if (string.IsNullOrEmpty(databaseType))
                throw new ArgumentException("Database type cannot be empty");

            switch (databaseType)
            {
                case DatabaseType.MYSQL:
                    if (isProduction)
                        _services.AddDbContextMysqlProduction(_configuration.ConnectionStrings.DatabaseConnectionString);
                    else
                        _services.AddDbContextMysqlDebug(_configuration.ConnectionStrings.DatabaseConnectionString);
                    break;

                case DatabaseType.SQLITE:
                    _services.AddDbContextSqlite(_configuration.ConnectionStrings.SQLiteConnectionString);
                    break;

                default:
                    throw new Exception("Invalid database, please provide correct value in appsettings.json");
            }

            return this;
        }

        public ModuleConfiguration ConfigureControllersWithViews()
        {
            _services.AddControllersWithViews()
                     .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ;

            return this;
        }

        public ModuleConfiguration ConfigureSecurity()
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
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "",
                    ValidAudience = "",
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.JWT.Key)),

                    ClockSkew = TimeSpan.Zero
                };

                options.Authority = "";
                // options.Authority = "https://localhost:5001";
                // options.Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration();
            });


            // TokenValidationParameters jw = new()
            // {
            //     ValidateIssuer = false,
            //     ValidateAudience = false,
            //     ValidateLifetime = true,
            //     ValidateIssuerSigningKey = true,
            //     ValidIssuer = "",
            //     ValidAudience = "",
            //     RequireExpirationTime = true,
            //     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.JWT.Key)),

            //     ClockSkew = TimeSpan.Zero
            // };

            IdentityModelEventSource.ShowPII = true;

            return this;
        }

        public ModuleConfiguration ConfigureSpa()
        {
            return this;
        }

        public ModuleConfiguration ConfigureServices()
        {
            _services.AddScoped<IJwtAuthentication, JwtAuthentication>();
            _services.AddScoped<IUserService, UserService>();
            _services.AddScoped<ICategoryRepository, CategoryRepository>();
            _services.AddScoped<ICategoryEntryRepository, CategoryEntryRepository>();

            // guide 
            GuideService _guide = new();
            _services.AddSingleton(_guide);

            return this;
        }

        public ModuleConfiguration ConfigureCors()
        {
            var origins = _configuration.Origins.ToArray();
            _services.AddCors(o => o.AddDefaultPolicy(builder =>
               {
                   builder.WithOrigins(origins)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
               }));

            return this;
        }

        public ModuleConfiguration ConfigureForwardedHeaders()
        {
            var type = _configuration.Network.Type;

            if (type == NetworkType.NGINX)
            {
                _services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                });
            }

            return this;
        }

        public ModuleConfiguration ConfigureSwagger()
        {
            _services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "App", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return this;
        }

        public IServiceCollection Build() => _services;
    }
}