using Core.Entities.Options;
using Microsoft.Extensions.Options;
using MTS.App;
using MTS.App.Configuration;
using MTS.App.Middlewares;
using MTS.Core.lib;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

Logo.PrintLogo();

var logsPath = Path.Combine(AppContext.BaseDirectory, @"logs");
if (!Directory.Exists(logsPath))
    Directory.CreateDirectory(logsPath);

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

LogOptions loggerOptions = new();
config.GetSection(LogOptions.Log).Bind(loggerOptions);

LogEventLevel logLevel = SerilogConfigurator.GetLogEventLevel(loggerOptions.LogLevel);
LogEventLevel logLevelSystem = SerilogConfigurator.GetLogEventLevel(loggerOptions.SystemLevel);
LogEventLevel logLevelAspNetCore = SerilogConfigurator.GetLogEventLevel(loggerOptions.AspNetCoreLevel);
LogEventLevel logLevelDatabase = SerilogConfigurator.GetLogEventLevel(loggerOptions.DatabaseLevel);

RollingInterval logInterval = SerilogConfigurator.GetRollingInterval(loggerOptions.TimeInterval);

builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Override("System", logLevelSystem)
    .MinimumLevel.Override("Microsoft.AspNetCore", logLevelAspNetCore)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", logLevelDatabase)
    .WriteTo.Async(e => e.Console(theme: AnsiConsoleTheme.Code))
    .WriteTo.Async(e => e.File(Path.Combine(logsPath, "log.txt"), rollingInterval: logInterval)));

_ = new ServicesConfigurator(builder.Services, config)
    .AddOptions()
    .ConfigureServices()
    .ConfigureControllersWithViews()
    .ConfigureDatabase(builder.Environment.IsProduction())
    .ConfigureSecurity()
    .ConfigureCors()
    .ConfigureSwagger()
    .ConfigureForwardedHeaders()
    .ConfigureHttpClients()
    .Build();

var app = builder.Build();
var networkOptions = app.Services.GetRequiredService<IOptions<NetworkOptions>>().Value;

app.EnsureDatabaseFolder();
await app.MigrateAsync();
await app.SeedIdentity();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    if (networkOptions.UseHSTS) app.UseHsts();
    if (networkOptions.HttpsRedirection) app.UseHttpsRedirection();
}

app.UseErrorHandler();
app.UseCors();
app.UseForwardedHeaders();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
if (logLevelAspNetCore > LogEventLevel.Information) app.UseSerilogRequestLogging();
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