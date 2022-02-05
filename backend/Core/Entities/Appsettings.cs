using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Constants;
using Microsoft.Extensions.Logging;

namespace Core.Entities
{
    public class Logger
    {
        public string LogLevel { get; set; }
        public string TimeInterval { get; set; }
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
        public int AccessTokenExpireTime { get; set; }
        public int RefreshTokenExpireTime { get; set; }
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
        public bool UseHttps { get; set; }
        public bool UseHSTS { get; set; }
        public bool HttpsRedirection { get; set; }
    }

    public class AppSettingsRoot
    {
        public Logger Logger { get; set; }
        public string AllowedHosts { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public List<string> Origins { get; set; }
        public JWT JWT { get; set; }
        public Database Database { get; set; }
        public Network Network { get; set; }

        [JsonIgnore]
        public static string FILE_NAME = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        [JsonIgnore]
        public static bool IsCreated
            => File.Exists(FILE_NAME);

        [JsonIgnore]
        public static string SavePath
            => Path.Combine(AppContext.BaseDirectory, @"save\save.sqlite");

        public static AppSettingsRoot Create()
        {
            if (IsCreated)
                return Load();

            // Might trigger antivirus for some reason?
            if (!Directory.Exists(Path.Combine(AppContext.BaseDirectory, "save")))
                Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "save"));

            var config = new AppSettingsRoot()
            {
                Logger = new Logger()
                {
                    LogLevel = "Information",
                    TimeInterval = "hour"
                },
                AllowedHosts = "*",
                ConnectionStrings = new ConnectionStrings()
                {
                    DatabaseConnectionString = "server=;uid=;pwd=;database=;",
                    SQLiteConnectionString = $"Data Source={SavePath}"
                },
                Origins = new List<string>() { "http://*/", "https://*/" },
                JWT = new JWT()
                {
                    Audience = "MyDomain.com",
                    Issuer = "MyDomain.com",
                    Key = CreateJwtKey(),
                    AccessTokenExpireTime = 30,
                    RefreshTokenExpireTime = 7200
                },
                Database = new Database()
                {
                    Type = DatabaseType.SQLITE
                },
                Network = new Network()
                {
                    HttpPort = "80",
                    HttpsPort = "443",
                    UseHttps = false,
                    HttpsRedirection = false,
                    UseHSTS = false,
                    Type = NetworkType.STANDALONE
                }
            };

            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            File.WriteAllBytes(FILE_NAME, JsonSerializer.SerializeToUtf8Bytes(config, options));

            if (!ValidateSettings(config))
                throw new Exception("Config is incorrect");

            return config;
        }

        public static AppSettingsRoot Load()
        {
            var readBytes = File.ReadAllBytes(FILE_NAME);
            var config = JsonSerializer.Deserialize<AppSettingsRoot>(readBytes);

            if (!ValidateSettings(config))
                throw new Exception("Config is incorrect");

            return config;
        }

        private static string CreateJwtKey()
        {

            Random random = new();
            int length = 64;
            var result = "";
            for (var i = 0; i < length; i++)
            {
                result += ((char)(random.Next(1, 26) + 64)).ToString().ToLower();
            }

            return result;
        }

        private static bool ValidateSettings(AppSettingsRoot settings)
        {


            return true;
        }
    }
}