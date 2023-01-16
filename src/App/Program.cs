using Core.Entities.Options;
using Microsoft.Extensions.Options;
using MTS.App;
using MTS.App.Configuration;
using MTS.App.Middlewares;
using MTS.Core.Entities;
using MTS.Core.lib;
using Serilog;
using Serilog.Events;

Logo.PrintLogo();

var logsPath = Path.Combine(AppContext.BaseDirectory, @"logs");
if (!Directory.Exists(logsPath))
    Directory.CreateDirectory(logsPath);

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

LogOptions loggerOptions = new();
config.GetSection(LogOptions.Log).Bind(loggerOptions);
LogEventLevel logLevel = SerilogConfigurator.GetLogEventLevel(loggerOptions.LogLevel);
RollingInterval logInterval = SerilogConfigurator.GetRollingInterval(loggerOptions.TimeInterval);

builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Override("Microsoft", logLevel)
    .WriteTo.Async(e => e.Console())
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