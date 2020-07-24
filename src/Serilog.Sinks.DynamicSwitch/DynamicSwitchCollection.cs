namespace Serilog.Sinks.DynamicSwitch
{
    using System;
    using System.Collections.Concurrent;
    using Core;

    public class DynamicSwitchCollection : ConcurrentDictionary<string, LoggingLevelSwitch>
    {
        private static readonly Lazy<DynamicSwitchCollection> LoggingLevelControllerSinks =
            new Lazy<DynamicSwitchCollection>(() =>
                new DynamicSwitchCollection());

        public static DynamicSwitchCollection Current => LoggingLevelControllerSinks.Value;
    }
}