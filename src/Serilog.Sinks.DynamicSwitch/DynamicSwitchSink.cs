namespace Serilog.Sinks.DynamicSwitch
{
    using System;
    using Core;
    using Events;

    // https://github.com/serilog/serilog-settings-configuration/issues/69
    // https://github.com/tsimbalar/serilog-settings-configuration/blob/12f563e16531f544024b592c0f5cce0938061fce/test/Serilog.Settings.Configuration.Tests/ConfigurationSettingsTests.cs#L285
    // https://github.com/serilog/serilog-settings-configuration/blob/6a56899ff23251bb4d6cb54355e69e11e0226f30/test/TestDummies/DummyWithLevelSwitchSink.cs
    public class DynamicSwitchSink : ILogEventSink
    {
        public DynamicSwitchSink(string name)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public void Emit(LogEvent logEvent)
        {
            // Do nothing.
        }
    }
}