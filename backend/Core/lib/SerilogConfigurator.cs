using System.Reflection.Metadata.Ecma335;
using Core.Entities;
using Serilog;
using Serilog.Events;

namespace Core.lib
{
    public static class SerilogConfigurator
    {
        public static LogEventLevel GetLogEventLevel(AppSettingsRoot settings)
        {
            return settings.Logger.LogLevel.ToLower() switch
            {
                "verbose" => LogEventLevel.Verbose,
                "debug" => LogEventLevel.Debug,
                "information" => LogEventLevel.Information,
                "warning" => LogEventLevel.Warning,
                "error" => LogEventLevel.Error,
                "fatal" => LogEventLevel.Fatal,
                _ => LogEventLevel.Warning
            };
        }

        public static RollingInterval GetRollingInterval(AppSettingsRoot settings)
        {
            return settings.Logger.TimeInterval.ToLower() switch
            {
                "minute" => RollingInterval.Minute,
                "hour" => RollingInterval.Hour,
                "day" => RollingInterval.Day,
                "month" => RollingInterval.Month,
                "year" => RollingInterval.Year,
                "infinite" => RollingInterval.Infinite,
                _ => RollingInterval.Hour
            };
        }
    }
}