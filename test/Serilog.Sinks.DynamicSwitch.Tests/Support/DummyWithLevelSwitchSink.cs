namespace Serilog.Sinks.DynamicSwitch.Tests.Support
{
    using System;
    using System.Collections.Generic;
    using Core;
    using Events;

    public class DummyWithLevelSwitchSink : ILogEventSink
    {
        [ThreadStatic]
        public static LoggingLevelSwitch ControlLevelSwitch;

        [ThreadStatic]
        // ReSharper disable ThreadStaticFieldHasInitializer
        public static List<LogEvent> Emitted = new List<LogEvent>();
        // ReSharper restore ThreadStaticFieldHasInitializer

        public DummyWithLevelSwitchSink(LoggingLevelSwitch loggingControlLevelSwitch)
        {
            ControlLevelSwitch = loggingControlLevelSwitch;
        }

        public void Emit(LogEvent logEvent)
        {
            Emitted.Add(logEvent);
        }
    }
}