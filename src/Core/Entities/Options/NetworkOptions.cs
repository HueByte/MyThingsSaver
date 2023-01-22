namespace Core.Entities.Options
{
    public class NetworkOptions
    {
        public const string Network = "Network";

        public string Type { get; set; } = string.Empty;
        public bool UseHttps { get; set; }
        public bool UseHSTS { get; set; }
        public bool HttpsRedirection { get; set; }
    }
}