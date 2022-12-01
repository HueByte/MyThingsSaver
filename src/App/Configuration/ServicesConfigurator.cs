using System.Text;
using System.Text.Json.Serialization;
using Common.Constants;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.Services.Authentication;
using Core.Services.Category;
using Core.Services.CurrentUser;
using Core.Services.Entry;
using Core.Services.Guide;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// composition root
namespace App.Configuration
{
    public class ServicesConfigurator
    {
        private readonly IServiceCollection _services;
        private readonly AppSettingsRoot _configuration;
        public ServicesConfigurator(IServiceCollection services, AppSettingsRoot configuration)
        {
            _services = services ?? new ServiceCollection();
            _configuration = configuration;
        }

        public ServicesConfigurator AddAppSettings()
        {
            _services.AddSingleton(_configuration);

            return this;
        }

        public ServicesConfigurator ConfigureDatabase(bool isProduction)
        {
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

        public ServicesConfigurator ConfigureControllersWithViews()
        {
            _services.AddControllersWithViews()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            _services.AddHttpContextAccessor();

            return this;
        }

        public ServicesConfigurator ConfigureSecurity()
        {
            _services.AddIdentity<ApplicationUserModel, IdentityRole>()
                     .AddEntityFrameworkStores<MTSContext>()
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
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

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
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    RequireExpirationTime = true,
                    ValidIssuer = _configuration.JWT.Issuer,
                    ValidAudience = _configuration.JWT.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.JWT.Key)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["X-Access-Token"];
                        return Task.CompletedTask;
                    },
                    // OnAuthenticationFailed = context =>
                    // {
                    //     Console.WriteLine(context.Exception.GetType() == typeof(SecurityTokenExpiredException));
                    //     return Task.CompletedTask;
                    // }
                };
            });

            return this;
        }

        public ServicesConfigurator ConfigureServices()
        {
            // services
            _services.AddScoped<ICategoryService, CategoryService>();
            _services.AddScoped<IEntryService, EntryService>();
            _services.AddScoped<ICurrentUserService, CurrentUserService>();
            _services.AddScoped<IJwtAuthentication, JwtAuthentication>();
            _services.AddScoped<IUserService, UserService>();
            _services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            // repositories
            _services.AddScoped<ICategoryRepository, CategoryRepository>();
            _services.AddScoped<IEntryRepository, EntryRepository>();
            _services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // guide 
            GuideService _guide = new();
            _services.AddSingleton(_guide);

            return this;
        }

        public ServicesConfigurator ConfigureCors()
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

        public ServicesConfigurator ConfigureForwardedHeaders()
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

        public ServicesConfigurator ConfigureSwagger()
        {
            _services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "App", Version = "v1" });

                c.AddSecurityDefinition("Cookie", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Cookie,
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