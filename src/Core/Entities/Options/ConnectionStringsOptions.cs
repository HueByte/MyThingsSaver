namespace Core.Entities.Options
{
    public class ConnectionStringsOptions
    {
        public const string ConnectionStrings = "ConnectionStrings";

        public string DatabaseConnectionString { get; set; } = string.Empty;
        public string SQLiteConnectionString { get; set; } = string.Empty;
    }
}