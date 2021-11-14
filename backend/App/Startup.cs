using System;
using System.Collections.Generic;
using App.Configuration;
using Common.Types;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
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
            // While migrating to .net 6 add .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IngoreCycles);
            services.AddControllersWithViews();


            var moduleConfiguration = new ModuleConfiguration(services, Configuration).ConfigureServices()
                                                                                      .ConfigureDatabase(_env.IsProduction())
                                                                                      .ConfigureSecurity()
                                                                                      .ConfigureCors(origins)
                                                                                      .ConfigureSpa()
                                                                                      .ConfigureSwagger()
                                                                                      .ConfigureForwardedHeaders();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // create ready database
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                // context.Database.EnsureCreated();
                context.Database.Migrate();
            }

            // seed roles & admin
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (!roleManager.RoleExistsAsync(Role.USER).GetAwaiter().GetResult())
                    roleManager.CreateAsync(new IdentityRole(Role.USER)).GetAwaiter().GetResult();

                if (!roleManager.RoleExistsAsync(Role.ADMIN).GetAwaiter().GetResult())
                    roleManager.CreateAsync(new IdentityRole(Role.ADMIN)).GetAwaiter().GetResult();

                var check = userManager.FindByNameAsync("admin").GetAwaiter().GetResult();
                if (check == null)
                {
                    ApplicationUser admin = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "admin",
                        Email = "admin@xyz.com"
                    };

                    // This is intended for now as default password for admin which will be required to be changed
                    var result = userManager.CreateAsync(admin, "Admin12").Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(admin, Role.ADMIN).Wait();
                    }
                }
            }

            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "App v1"));
            }
            else
            {
                bool doUseHSTS = Configuration.GetValue<bool>("Network:UseHSTS");
                bool doHttpsRedirect = Configuration.GetValue<bool>("Network:HttpsRedirection");

                if (doUseHSTS)
                    app.UseHsts();

                if (doHttpsRedirect)
                    app.UseHttpsRedirection();
            }

            app.UseCors();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

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

            app.UseStaticFiles();

            app.UseSpaStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = ctx =>
                {
                    var headers = ctx.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(10)
                    };
                }
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../../client/";
                spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
                {
                    OnPrepareResponse = ctx =>
                    {
                        var headers = ctx.Context.Response.GetTypedHeaders();
                        headers.CacheControl = new CacheControlHeaderValue
                        {
                            Public = true,
                            MaxAge = TimeSpan.FromDays(10)
                        };
                    }
                };

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
