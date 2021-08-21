using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Core.Entities
{
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

        [JsonIgnore]
        public static string FILE_NAME = AppContext.BaseDirectory + "appsettings.json";

        [JsonIgnore]
        public static bool IsCreated
            => File.Exists(FILE_NAME);

        [JsonIgnore]
        public static string SavePath
            => AppContext.BaseDirectory + @"\save\save.sqlite";

        // Checks if appsettings.json exist
        // if doesn't it seeds the data
        // generates RSA key for JWT
        // creates folder for save.sqlite which is created automatically
        // sets default ports :80 for http and :443 for https
        // it is expected that during debug appsettings.json should be duplicated from project folder 
        // instead of creating new one in /debug 
        public static AppSettingsRoot Create()
        {
            if (IsCreated)
                return Load();

            // Might trigger antivirus for some reason?
            if (!Directory.Exists(AppContext.BaseDirectory + @"\save"))
                Directory.CreateDirectory(AppContext.BaseDirectory + @"\save");

            var config = new AppSettingsRoot()
            {
                Logging = new Logging()
                {
                    LogLevel = new LogLevel() { Default = Microsoft.Extensions.Logging.LogLevel.Warning.ToString() }
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
                    Audience = "My domain",
                    Issuer = "My domain",
                    Key = CreateJwtKey()
                },
                Database = new Database()
                {
                    Type = "SQLite"
                },
                Network = new Network()
                {
                    HttpPort = "80",
                    HttpsPort = "443",
                    HttpsRedirection = false,
                    Type = "standalone"
                }
            };

            JsonSerializerOptions options = new()
            {
                WriteIndented = true
            };

            File.WriteAllBytes(FILE_NAME, JsonSerializer.SerializeToUtf8Bytes(config, options));

            return config;
        }

        public static AppSettingsRoot Load()
        {
            var readBytes = File.ReadAllBytes(FILE_NAME);
            var config = JsonSerializer.Deserialize<AppSettingsRoot>(readBytes);
            return config;
        }

        private static string CreateJwtKey()
        {
            var rsa = new RSACryptoServiceProvider(1024);
            var result = ExportPrivateKey(rsa);
            return result;
        }

        private static string ExportPrivateKey(RSACryptoServiceProvider scp)
        {
            string result;

            if (scp.PublicOnly)
            {
                throw new ArgumentException("CSP does not contain a private key");
            }

            var parameters = scp.ExportParameters(true);

            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
                    EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent);
                    EncodeIntegerBigEndian(innerWriter, parameters.D);
                    EncodeIntegerBigEndian(innerWriter, parameters.P);
                    EncodeIntegerBigEndian(innerWriter, parameters.Q);
                    EncodeIntegerBigEndian(innerWriter, parameters.DP);
                    EncodeIntegerBigEndian(innerWriter, parameters.DQ);
                    EncodeIntegerBigEndian(innerWriter, parameters.InverseQ);
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                result = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
            }

            return result;
        }

        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }
    }
}