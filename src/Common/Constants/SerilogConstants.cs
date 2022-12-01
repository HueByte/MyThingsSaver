using System.IO;

namespace MTS.Common.Constants
{
    public class SerilogConstants
    {
        public partial class LogLevels
        {
            public static string[] Levels = { Verbose, Debug, Warning, Information, Error, Fatal };
            public const string Verbose = "verbose";
            public const string Debug = "debug";
            public const string Warning = "warning";
            public const string Information = "information";
            public const string Error = "error";
            public const string Fatal = "fatal";
        }

        public partial class TimeIntervals
        {
            public static string[] Intervals = { Minute, Hour, Day, Month, Year, Infinite };
            public const string Minute = "minute";
            public const string Hour = "hour";
            public const string Day = "day";
            public const string Month = "month";
            public const string Year = "year";
            public const string Infinite = "infinite";
        }
    }
}