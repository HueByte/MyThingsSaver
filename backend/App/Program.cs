using System;
using Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace App
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            AppSettingsRoot appsettings = AppSettingsRoot.IsCreated
                ? AppSettingsRoot.Load()
                : AppSettingsRoot.Create();

            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

                    var httpPort = config.GetValue<string>("Network:HttpPort");
                    var httpsPort = config.GetValue<string>("Network:HttpsPort");

                    webBuilder.UseUrls($"http://0.0.0.0:{httpPort};https://0.0.0.0:{httpsPort}");
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
