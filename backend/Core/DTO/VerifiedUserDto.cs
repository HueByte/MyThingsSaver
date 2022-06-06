using System;
using System.Text.Json.Serialization;
using Core.Models;

namespace Core.DTO
{
    public class VerifiedUserDto
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

        public VerifiedUserDto() { }

        public VerifiedUserDto(ApplicationUser user, string[] roles, string jwtToken, RefreshToken refreshToken, DateTime accessTokenExireDate)
        {
            Username = user.UserName;
            Roles = roles;
            Token = jwtToken;
            RefreshToken = refreshToken.Token;
            RefreshTokenExpiration = refreshToken.Expires;
            AccessTokenExpiration = accessTokenExireDate;
        }
    }
}