using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using App.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;

namespace App.Controllers
{
    public class ConfigurationController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        public ConfigurationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private bool CheckIfCanAccess(HttpContext httpContext)
        {
            var connection = httpContext.Connection;
            bool canAccess = false;

            if (connection.RemoteIpAddress != null)
            {
                if (connection.LocalIpAddress != null)
                {
                    // check if localhost
                    canAccess = connection.RemoteIpAddress.Equals(connection.LocalIpAddress);
                }
                else
                {
                    canAccess = IPAddress.IsLoopback(connection.RemoteIpAddress);
                }
            }

            return canAccess;
        }

        private static async Task<string> GetAppSettingsJsonAsync()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            return await System.IO.File.ReadAllTextAsync(filePath);
        }

        private static async Task WriteSettingsAsync(string settings)
        {
            try
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                await System.IO.File.WriteAllTextAsync(filePath, settings);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [HttpGet("GetConfiguration")]
        [Authorize]
        public async Task<IActionResult> GetConfig()
        {
            var canAccess = CheckIfCanAccess(this.HttpContext);

            if (canAccess)
            {
                var result = await GetAppSettingsJsonAsync();
                return Ok(result);
            }
            else
                return Unauthorized();
        }

        [HttpPost("ChangeConfiguration")]
        [Authorize]
        public async Task<IActionResult> ChangeSettings([FromBody] AppSettingsRoot settings)
        {
            var canAccess = CheckIfCanAccess(this.HttpContext);

            if (canAccess)
            {
                JsonSerializerOptions options = new()
                {
                    WriteIndented = true
                };

                var settingsJson = System.Text.Json.JsonSerializer.Serialize(settings, options);

                await WriteSettingsAsync(settingsJson);
                return Ok();
            }
            else
                return Unauthorized();
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class LogLevel
    {
        public string Default { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class ConnectionStrings
    {
        public string DatabaseConnectionString { get; set; }
        public string SQLiteConnectionString { get; set; }
    }

    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }

    public class Database
    {
        public string Type { get; set; }
    }

    public class Network
    {
        public string Type { get; set; }
        public string HttpPort { get; set; }
        public string HttpsPort { get; set; }
        public bool HttpsRedirection { get; set; }
    }

    public class AppSettingsRoot
    {
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public List<string> Origins { get; set; }
        public JWT JWT { get; set; }
        public Database Database { get; set; }
        public Network Network { get; set; }
    }
}