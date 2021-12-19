// using System;
// using Core.Entities;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Hosting;

// namespace App
// {
//     public class Program
//     {
//         public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

//         public static IHostBuilder CreateHostBuilder(string[] args)
//         {
//             // TODO : Remove Load() as it's loaded via config anyway
//             AppSettingsRoot appsettings = AppSettingsRoot.IsCreated
//                 ? AppSettingsRoot.Load()
//                 : AppSettingsRoot.Create();

//             return Host.CreateDefaultBuilder(args)
//                 .ConfigureAppConfiguration((hostingContext, config) =>
//                 {
//                     config.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json", optional: false, reloadOnChange: true);
//                 })
//                 .ConfigureWebHostDefaults(webBuilder =>
//                 {
//                     var config = new ConfigurationBuilder()
//                         .AddJsonFile("appsettings.json")
//                         .Build();

//                     var useHttps = config.GetValue<bool>("Network:UseHttps");
//                     var httpPort = config.GetValue<string>("Network:HttpPort");
//                     var httpsPort = config.GetValue<string>("Network:HttpsPort");

//                     if (useHttps)
//                         webBuilder.UseUrls($"http://0.0.0.0:{httpPort};https://0.0.0.0:{httpsPort}");
//                     else
//                         webBuilder.UseUrls($"http://0.0.0.0:{httpPort}");

//                     webBuilder.UseStartup<Startup>();
//                 });
//         }
//     }
// }

using App.Configuration;
using Common.Types;
using Core.Entities;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


AppSettingsRoot appsettings = AppSettingsRoot.IsCreated
    ? AppSettingsRoot.Load()
    : AppSettingsRoot.Create();

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json", optional: false, reloadOnChange: true);
});

builder.Host.ConfigureServices(services =>
{
    services = new ModuleConfiguration(services, appsettings).ConfigureServices()
                                                             .ConfigureControllersWithViews()
                                                             .ConfigureDatabase(builder.Environment.IsProduction())
                                                             .ConfigureSecurity()
                                                             .ConfigureCors()
                                                             .ConfigureSpa()
                                                             .ConfigureSwagger()
                                                             .ConfigureForwardedHeaders()
                                                             .Build();
});

var app = builder.Build();

// create ready database
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    // context.Database.EnsureCreated();
    context.Database.Migrate();
}

// seed roles & admin
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
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

// configue app
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "App v1"));
}

else
{
    bool doUseHSTS = appsettings.Network.UseHSTS;
    bool doHttpsRedirect = appsettings.Network.HttpsRedirection;

    if (doUseHSTS)
        app.UseHsts();

    if (doHttpsRedirect)
        app.UseHttpsRedirection();
}

app.UseHttpLogging();
app.UseCors();
app.UseForwardedHeaders();

app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = ctx =>
    {
        var headerse = ctx.Context.Response.GetTypedHeaders();
        headerse.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
        {
            Public = true,
            MaxAge = TimeSpan.FromDays(10)
        };
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthentication();

app.MapControllers();
app.MapFallbackToFile("index.html");

var useHttps = appsettings.Network.UseHttps;
var httpPort = appsettings.Network.HttpPort;
var httpsPort = appsettings.Network.HttpsPort;

if (useHttps)
{
    app.Urls.Add($"http://0.0.0.0:{httpPort}");
    app.Urls.Add($"https://0.0.0.0:{httpsPort}");
}
else
{
    app.Urls.Add($"http://0.0.0.0:{httpPort}");
}

app.Run();