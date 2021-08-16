using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace App
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
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
