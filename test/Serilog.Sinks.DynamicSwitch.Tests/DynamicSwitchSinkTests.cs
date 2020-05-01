namespace Serilog.Sinks.DynamicSwitch.Tests
{
    using Core;
    using Events;
    using Xunit;
    using Xunit.Categories;

    public class DynamicSwitchSinkTests
    {
        private const string SwitchName = "switch";
        private const LogEventLevel LogLevel = LogEventLevel.Fatal;

        [UnitTest]
        [Fact]
        public void ConstructDynamicSwitchSink()
        {
            var sink = new DynamicSwitchSink(SwitchName, new LoggingLevelSwitch(LogLevel));

            Assert.Equal(sink.Name, SwitchName);
            Assert.Equal(sink.Switch.MinimumLevel, LogLevel);
        }
    }
}