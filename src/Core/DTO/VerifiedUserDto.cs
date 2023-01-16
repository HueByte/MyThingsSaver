using System;
using System.Text.Json.Serialization;
using MTS.Core.Models;

namespace MTS.Core.DTO
{
    public class VerifiedUserDto
    {
        public string? Username { get; set; }
        public string[]? Roles { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Email { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        [JsonIgnore]
        public DateTime RefreshTokenExpiration { get; set; }

        [JsonIgnore]
        public string? Token { get; set; }

        [JsonIgnore]
        public DateTime AccessTokenExpiration { get; set; }

        public VerifiedUserDto() { }

        public VerifiedUserDto(ApplicationUserModel user, string[] roles, string jwtToken, string email, RefreshTokenModel refreshToken, DateTime accessTokenExireDate)
        {
            Username = user.UserName;
            Roles = roles;
            Token = jwtToken;
            Email = email;
            RefreshToken = refreshToken.Token;
            RefreshTokenExpiration = refreshToken.Expires;
            AccessTokenExpiration = accessTokenExireDate;
        }
    }
}