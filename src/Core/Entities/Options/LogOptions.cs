namespace Core.Entities.Options
{
    public class LogOptions
    {
        public const string Log = "Log";

        public string LogLevel { get; set; } = string.Empty;
        public string TimeInterval { get; set; } = string.Empty;
    }
}