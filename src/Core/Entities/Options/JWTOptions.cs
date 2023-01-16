namespace Core.Entities.Options
{
    public class JWTOptions
    {
        public const string JWT = "JWT";

        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public double AccessTokenExpireTime { get; set; }
        public double RefreshTokenExpireTime { get; set; }
    }
}