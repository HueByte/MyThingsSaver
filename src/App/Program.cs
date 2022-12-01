using MTS.App;
using MTS.App.Configuration;
using MTS.App.Middlewares;
using MTS.Core.Entities;
using MTS.Core.lib;
using Serilog;
using Serilog.Events;

Logo.PrintLogo();

if (!Directory.Exists(Path.Combine(AppContext.BaseDirectory, @"logs")))
    Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, @"logs"));

AppSettingsRoot appsettings = AppSettingsRoot.IsCreated
    ? AppSettingsRoot.Load()
    : AppSettingsRoot.Create();

LogEventLevel logLevel = SerilogConfigurator.GetLogEventLevel(appsettings);
RollingInterval logInterval = SerilogConfigurator.GetRollingInterval(appsettings);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Override("Microsoft", logLevel)
    .WriteTo.Async(e => e.Console())
    .WriteTo.Async(e => e.File(Path.Combine(AppContext.BaseDirectory, "logs/log.txt"), rollingInterval: logInterval)));

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json", optional: false, reloadOnChange: true);
});

builder.Host.ConfigureServices(services =>
{
    _ = new ServicesConfigurator(services, appsettings).AddAppSettings()
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

await app.Migrate();
await app.SeedIdentity();

// configue app
app.UseErrorHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
app.UseCookiePolicy();
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
app.UseCurrentUser();
app.MapGet("/api", () => "Hello World");
app.MapFallbackToFile("index.html");
app.Run();