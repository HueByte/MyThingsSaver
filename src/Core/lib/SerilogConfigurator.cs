using System.Reflection.Metadata.Ecma335;
using MTS.Common.Constants;
using MTS.Core.Entities;
using Serilog;
using Serilog.Events;

namespace MTS.Core.lib
{
    public static class SerilogConfigurator
    {
        public static LogEventLevel GetLogEventLevel(string? setting)
        {
            if (string.IsNullOrEmpty(setting)) return LogEventLevel.Warning;

            return setting.ToLower() switch
            {
                SerilogConstants.LogLevels.Verbose => LogEventLevel.Verbose,
                SerilogConstants.LogLevels.Debug => LogEventLevel.Debug,
                SerilogConstants.LogLevels.Information => LogEventLevel.Information,
                SerilogConstants.LogLevels.Warning => LogEventLevel.Warning,
                SerilogConstants.LogLevels.Error => LogEventLevel.Error,
                SerilogConstants.LogLevels.Fatal => LogEventLevel.Fatal,
                _ => LogEventLevel.Warning
            };
        }

        public static RollingInterval GetRollingInterval(string? setting)
        {
            if (string.IsNullOrEmpty(setting)) return RollingInterval.Day;

            return setting.ToLower() switch
            {
                SerilogConstants.TimeIntervals.Minute => RollingInterval.Minute,
                SerilogConstants.TimeIntervals.Hour => RollingInterval.Hour,
                SerilogConstants.TimeIntervals.Day => RollingInterval.Day,
                SerilogConstants.TimeIntervals.Month => RollingInterval.Month,
                SerilogConstants.TimeIntervals.Year => RollingInterval.Year,
                SerilogConstants.TimeIntervals.Infinite => RollingInterval.Infinite,
                _ => RollingInterval.Hour
            };
        }
    }
}