using System.Text;
using System.Text.Json.Serialization;
using Common.Constants;
using Core.Entities.Options;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services.LoginLog;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MTS.Core.Services.Authentication;
using MTS.Core.Services.Category;
using MTS.Core.Services.CurrentUser;
using MTS.Core.Services.Entry;
using MTS.Core.Services.Guide;
using MTS.Core.Services.LegalNotice;
using MTS.Infrastructure;
using MTS.Infrastructure.Repositories;
using MTS.Infrastructure.Services.Geolocation;

// composition root
namespace MTS.App.Configuration
{
    public class ServicesConfigurator
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _config;
        public ServicesConfigurator(IServiceCollection services, IConfiguration config)
        {
            _services = services ?? new ServiceCollection();
            _config = config;
        }

        public ServicesConfigurator AddOptions()
        {
            _services.Configure<ConnectionStringsOptions>(options => _config.GetSection(ConnectionStringsOptions.ConnectionStrings).Bind(options));
            _services.Configure<JWTOptions>(options => _config.GetSection(JWTOptions.JWT).Bind(options));
            _services.Configure<DatabaseOptions>(options => _config.GetSection(DatabaseOptions.Database).Bind(options));
            _services.Configure<NetworkOptions>(options => _config.GetSection(NetworkOptions.Network).Bind(options));
            _services.Configure<LogOptions>(options => _config.GetSection(LogOptions.Log).Bind(options));

            return this;
        }

        public ServicesConfigurator ConfigureDatabase(bool isProduction)
        {
            DatabaseOptions? databaseOptions = _config.GetSection(DatabaseOptions.Database).Get<DatabaseOptions>();
            ConnectionStringsOptions? connectionStringsOptions = _config.GetSection(ConnectionStringsOptions.ConnectionStrings).Get<ConnectionStringsOptions>();

            if (databaseOptions == null)
                throw new Exception("Database options cannot be null");

            if (connectionStringsOptions == null)
                throw new Exception("Connection strings options cannot be null");

            var databaseType = databaseOptions?.Type.ToLower();

            if (string.IsNullOrEmpty(databaseType))
                throw new Exception("Database type cannot be empty");

            switch (databaseType)
            {
                case DatabaseType.MYSQL:
                    if (isProduction)
                        _services.AddDbContextMysqlProduction(connectionStringsOptions.DatabaseConnectionString);
                    else
                        _services.AddDbContextMysqlDebug(connectionStringsOptions.DatabaseConnectionString);
                    break;

                case DatabaseType.SQLITE:
                default:
                    _services.AddDbContextSqlite(connectionStringsOptions.SQLiteConnectionString);
                    break;
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
            var jwtOptions = _config.GetSection(JWTOptions.JWT).Get<JWTOptions>();

            if (jwtOptions is null)
                throw new Exception("JWT options are not configured");

            _services.AddIdentity<ApplicationUserModel, RoleModel>()
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
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["X-Access-Token"];
                        return Task.CompletedTask;
                    },
                };
            });

            return this;
        }

        public ServicesConfigurator ConfigureHttpClients()
        {
            _services.AddHttpClient(HttpClientNames.GEO_LOCATION_CLIENT, client =>
            {
                client.BaseAddress = new Uri("http://ip-api.com/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            return this;
        }

        public ServicesConfigurator ConfigureServices()
        {
            // Services
            _services.AddScoped<ICategoryService, CategoryService>();
            _services.AddScoped<IEntryService, EntryService>();
            _services.AddScoped<ICurrentUserService, CurrentUserService>();
            _services.AddScoped<IJwtAuthentication, JwtAuthentication>();
            _services.AddScoped<IUserService, UserService>();
            _services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            _services.AddScoped<ILoginLogService, LoginLogService>();
            _services.AddScoped<IGeolocationService, GeolocationService>();

            // Repositories
            _services.AddScoped<ICategoryRepository, CategoryRepository>();
            _services.AddScoped<IEntryRepository, EntryRepository>();
            _services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            _services.AddScoped<ILoginLogRepository, LoginLogRepository>();

            // Guide 
            GuideService _guide = new();
            _services.AddSingleton(_guide);

            // Privacy policy
            LegalNoticeService _privacyPolicy = new();
            _services.AddSingleton(_privacyPolicy);

            return this;
        }

        public ServicesConfigurator ConfigureCors()
        {
            var origins = _config.GetSection("Origins").Get<string[]>();

            if (origins is null)
                throw new Exception("Origins are not configured");

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
            var networkOptions = _config.GetSection(NetworkOptions.Network).Get<NetworkOptions>();

            if (networkOptions is null)
                throw new Exception("Network options are not configured");

            var type = networkOptions.Type.ToLower();

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MTS", Version = "v1" });
            });

            return this;
        }

        public IServiceCollection Build() => _services;
    }
}