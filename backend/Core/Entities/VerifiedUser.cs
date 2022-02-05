using System;
using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class VerifiedUser
    {
        // public string Token { get; set; }
        // public string TokenType { get; set; }
        public string Username { get; set; }
        public string[] Roles { get; set; }
        // public DateTime ExpireDate { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }

        [JsonIgnore]
        public DateTime RefreshTokenExpiration { get; set; }

        [JsonIgnore]
        public string Token { get; set; }

        [JsonIgnore]
        public DateTime AccessTokenExpiration { get; set; }
        // public DateTime RefreshTokenExpiration { get; set; }
    }
}