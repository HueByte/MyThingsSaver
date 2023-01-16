using System.Text.Json;
using System.Text.Json.Serialization;
using MTS.Common.Constants;

namespace MTS.Core.Entities
{
    public class AppSettingsRoot
    {
        public Logger Logger { get; set; } = default!;
        public string AllowedHosts { get; set; } = default!;
        public ConnectionStrings ConnectionStrings { get; set; } = default!;
        public List<string> Origins { get; set; } = default!;
        public JWT JWT { get; set; } = default!;
        public Database Database { get; set; } = default!;
        public Network Network { get; set; } = default!;

        [JsonIgnore]
        public static string FILE_NAME = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        [JsonIgnore]
        public static bool IsCreated
            => File.Exists(FILE_NAME);

        [JsonIgnore]
        public static string SavePath
            => Path.Combine(AppContext.BaseDirectory, @"save/save.sqlite");

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

            if (!ValidateSettings(config!))
                throw new Exception("Config is incorrect");

            return config!;
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
            List<string> errors = new();
            if (settings == null)
                errors.Add("Config is empty");
            else
            {
                // validate logger
                if (!SerilogConstants.LogLevels.Levels.Contains(settings.Logger?.LogLevel.ToLower()))
                    errors.Add("Incorrect logger level");

                if (!SerilogConstants.TimeIntervals.Intervals.Contains(settings.Logger?.TimeInterval.ToLower()))
                    errors.Add("Incorrect logger time interval");

                // validate JWT
                if (string.IsNullOrEmpty(settings.JWT?.Key) || settings.JWT.Key.Length < 32)
                    errors.Add("Your JWT key is empty or is too short");

                if ((double)settings.JWT?.AccessTokenExpireTime! <= 0 || (int)settings.JWT.RefreshTokenExpireTime <= 0 || (double)settings.JWT.AccessTokenExpireTime > (double)settings.JWT.RefreshTokenExpireTime)
                    errors.Add("Your expire time values are incorrect, remember that {RefreshTokenexpireTime} should be bigger than {AccessTokenExpireTime}");

                // validate database
                if (settings.Database?.Type.ToLower() != DatabaseType.MYSQL && settings.Database?.Type.ToLower() != DatabaseType.SQLITE)
                    errors.Add("Incorrect Database type");

                // validate networking
                if (settings.Network?.Type.ToLower() != NetworkType.NGINX && settings.Network?.Type.ToLower() != NetworkType.STANDALONE)
                    errors.Add("Incorrect network type");

                if (string.IsNullOrEmpty(settings.Network?.HttpPort) || string.IsNullOrEmpty(settings.Network.HttpsPort))
                    errors.Add("There's something wrong with your ports config");

                if (errors.Count > 0)
                {
                    string message = string.Join("\n--- ", errors);
                    Console.WriteLine($"You got some config errors\n--- {message}");
                    throw new Exception("Config validation failed");
                }
            }

            return true;
        }
    }

    public class Logger
    {
        public string LogLevel { get; set; } = default!;
        public string TimeInterval { get; set; } = default!;
    }

    public class ConnectionStrings
    {
        public string DatabaseConnectionString { get; set; } = default!;
        public string SQLiteConnectionString { get; set; } = default!;
    }

    public class JWT
    {
        public string Key { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public double AccessTokenExpireTime { get; set; }
        public double RefreshTokenExpireTime { get; set; }
    }

    public class Database
    {
        public string Type { get; set; } = default!;
    }

    public class Network
    {
        public string Type { get; set; } = default!;
        public string HttpPort { get; set; } = default!;
        public string HttpsPort { get; set; } = default!;
        public bool UseHttps { get; set; }
        public bool UseHSTS { get; set; }
        public bool HttpsRedirection { get; set; }
    }

}