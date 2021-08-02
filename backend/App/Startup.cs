using System;
using App.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Load origins from appsettings.json
            string[] origins = Configuration.GetSection("Origins").Get<string[]>();

            services.AddRazorPages();
            // While updating to .net 6 add .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IngoreCycles);
            services.AddControllersWithViews();


            ModuleConfiguration moduleConfiguration = new(services, Configuration);
            moduleConfiguration.ConfigureServices();
            moduleConfiguration.ConfigureDatabase(_env.IsProduction());
            moduleConfiguration.ConfigureSecurity();
            moduleConfiguration.ConfigureCors(origins);
            moduleConfiguration.ConfigureSpa();

            services.AddSwaggerGen(c =>
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "App v1"));
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapGet("/api", async context =>
                {
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(@"
##     ##  #######     ##      ##     #####                 ##      ##   #####   ########     ##   ########  
##     ## ##     ##  ####    ####    ##   ##                ##  ##  ##  ##   ##  ##     ##  ####   ##     ## 
##     ##        ##    ##      ##   ##     ##               ##  ##  ## ##     ## ##     ##    ##   ##     ## 
#########  #######     ##      ##   ##     ##               ##  ##  ## ##     ## ########     ##   ##     ## 
##     ##        ##    ##      ##   ##     ##               ##  ##  ## ##     ## ##   ##      ##   ##     ## 
##     ## ##     ##    ##      ##    ##   ##                ##  ##  ##  ##   ##  ##    ##     ##   ##     ## 
##     ##  #######   ######  ######   #####                  ###  ###    #####   ##     ##  ###### ########  
                    ");
                });
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../../client/";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
