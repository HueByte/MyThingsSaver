using App;
using App.Configuration;
using Common.Constants;
using Core.Entities;
using Core.Models;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

if (!Directory.Exists(Path.Combine(AppContext.BaseDirectory, @"logs")))
    Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, @"logs"));


var builder = WebApplication.CreateBuilder(args);

AppSettingsRoot appsettings = AppSettingsRoot.IsCreated
    ? AppSettingsRoot.Load()
    : AppSettingsRoot.Create();

Log.Logger = new LoggerConfiguration().WriteTo.Console().WriteTo
                                              .File(Path.Combine(AppContext.BaseDirectory, @"logs\log.txt"))
                                              .CreateBootstrapLogger();

builder.Host.UseSerilog();

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json", optional: false, reloadOnChange: true);
});

builder.Host.ConfigureServices(services =>
{
    services = new ModuleConfiguration(services, appsettings).AddAppSettings()
                                                             .ConfigureServices()
                                                             .ConfigureControllersWithViews()
                                                             .ConfigureDatabase(builder.Environment.IsProduction())
                                                             .ConfigureSecurity()
                                                             .ConfigureCors()
                                                             .ConfigureSwagger()
                                                             .ConfigureForwardedHeaders()
                                                             .Build();
});

var app = builder.Build();

app = new BeforeStart(app).PerformMigrations()
                          .SeedIdentity()
                          .Initialize();

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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

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

app.MapControllers();

var useHttps = appsettings.Network.UseHttps;
var httpPort = appsettings.Network.HttpPort;
var httpsPort = appsettings.Network.HttpsPort;

if (useHttps)
{
    app.Urls.Add($"http://localhost:{httpPort}");
    app.Urls.Add($"https://localhost:{httpsPort}");
}
else
{
    app.Urls.Add($"http://localhost:{httpPort}");
}

app.MapGet("/api", () => "Hello World");

app.MapFallbackToFile("index.html");
app.Run();