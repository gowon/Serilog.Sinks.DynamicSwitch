namespace Serilog.Sinks.DynamicSwitch
{
    using System;
    using System.Collections.Concurrent;
    using Core;

    public class LoggingLevelSwitchCollection : ConcurrentDictionary<string, LoggingLevelSwitch>
    {
        private static readonly Lazy<LoggingLevelSwitchCollection> LoggingLevelControllerSinks =
            new Lazy<LoggingLevelSwitchCollection>(() =>
                new LoggingLevelSwitchCollection());

        public static LoggingLevelSwitchCollection Current => LoggingLevelControllerSinks.Value;
    }
}