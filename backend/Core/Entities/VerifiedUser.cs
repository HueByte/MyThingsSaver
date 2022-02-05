using System;
using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class VerifiedUser
    {
        public string Username { get; set; }
        public string[] Roles { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        [JsonIgnore]
        public DateTime RefreshTokenExpiration { get; set; }

        [JsonIgnore]
        public string Token { get; set; }

        [JsonIgnore]
        public DateTime AccessTokenExpiration { get; set; }
    }
}