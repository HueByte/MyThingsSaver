using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTS.App.Extensions;
using MTS.Core.Entities;

namespace MTS.App.Controllers
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
        [Authorize(Roles = Role.ADMIN)]
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
        [Authorize(Roles = Role.ADMIN)]
        public async Task<IActionResult> ChangeSettings([FromBody] AppSettingsRoot settings)
        {
            var canAccess = CheckIfCanAccess(this.HttpContext);

            if (canAccess)
            {
                JsonSerializerOptions options = new()
                {
                    WriteIndented = true
                };

                var settingsJson = JsonSerializer.Serialize(settings, options);

                await WriteSettingsAsync(settingsJson);
                return Ok();
            }
            else
                return Unauthorized();
        }
    }


}