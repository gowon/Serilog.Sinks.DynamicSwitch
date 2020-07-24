namespace Serilog.Sinks.DynamicSwitch.Tests
{
    using System;
    using Core;
    using Events;
    using Microsoft.Extensions.DependencyInjection;
    using Support;
    using Xunit;
    using Xunit.Categories;
    using static Support.LogEventHelper;

    public class DynamicSwitchSinkTests
    {
        [UnitTest]
        [Fact]
        public void AddMultipleDynamicSwitches()
        {
            const string switchName1 = "switch1";
            const string switchName2 = "switch2";
            var collection = new DynamicSwitchCollection();
            var logSwitch = new LoggingLevelSwitch(LogEventLevel.Error);
            var logSwitch2 = new LoggingLevelSwitch(LogEventLevel.Debug);

            var log = new LoggerConfiguration()
                .WriteTo.DynamicSwitch(collection, switchName1, logSwitch)
                .WriteTo.DynamicSwitch(collection, switchName2, logSwitch2)
                .CreateLogger();

            Assert.Equal(2, collection.Count);
            Assert.Equal(LogEventLevel.Error, collection[switchName1].MinimumLevel);
            Assert.Equal(LogEventLevel.Debug, collection[switchName2].MinimumLevel);
        }

        [UnitTest]
        [Fact]
        public void AddMultipleDynamicSwitches_ThrowOnDuplicateNameKey()
        {
            const string switchName = "switch";
            var collection = new DynamicSwitchCollection();
            var logSwitch = new LoggingLevelSwitch(LogEventLevel.Error);
            var logSwitch2 = new LoggingLevelSwitch(LogEventLevel.Debug);

            Assert.Throws<ArgumentException>(() =>
            {
                new LoggerConfiguration()
                    .WriteTo.DynamicSwitch(collection, switchName, logSwitch)
                    .WriteTo.DynamicSwitch(collection, switchName, logSwitch2)
                    .CreateLogger();
            });
        }

        [UnitTest]
        [Fact]
        public void AddMultipleDynamicSwitches2()
        {
            const string switchName1 = "switch1";
            const string switchName2 = "switch2";
            var collection = new DynamicSwitchCollection();
            var logSwitch = new LoggingLevelSwitch(LogEventLevel.Error);
            var logSwitch2 = new LoggingLevelSwitch(LogEventLevel.Debug);

            var log = new LoggerConfiguration()
                .WriteTo.DynamicSwitch(collection, switchName1, logSwitch)
                .WriteTo.DynamicSwitch(collection, switchName2, logSwitch2)
                .CreateLogger();

            Assert.Equal(2, collection.Count);
            Assert.Equal(LogEventLevel.Error, collection[switchName1].MinimumLevel);
            Assert.Equal(LogEventLevel.Debug, collection[switchName2].MinimumLevel);
        }

        [UnitTest]
        [Fact]
        public void UpdateSwitchFromCollection_ChangeEmitBehavior()
        {
            const string switchName = "switch";
            var collection = new DynamicSwitchCollection();
            var logSwitch = new LoggingLevelSwitch(LogEventLevel.Error);
            var a = LogEvent(LogEventLevel.Error, null, "Hello, {Name}!", "Alice");
            var b = LogEvent(LogEventLevel.Information, null, "Hello, {Name}!", "Bob");
            var c = LogEvent(LogEventLevel.Warning, null, "Hello, {Name}!", "Charlie");

            var log = new LoggerConfiguration()
                .WriteTo.Sink(new DummyWithLevelSwitchSink(logSwitch), levelSwitch: logSwitch)
                .WriteTo.DynamicSwitch(collection, switchName, logSwitch)
                .CreateLogger();

            log.Write(a);
            Assert.Single(DummyWithLevelSwitchSink.Emitted);

            log.Write(b);
            Assert.Single(DummyWithLevelSwitchSink.Emitted);

            collection[switchName].MinimumLevel = LogEventLevel.Debug;

            log.Write(c);
            Assert.Equal(2, DummyWithLevelSwitchSink.Emitted.Count);
        }

        [UnitTest]
        [Fact]
        public void UseSingletonCollectionDependencyInjection()
        {
            const string switchName = "switch";
            var logSwitch = new LoggingLevelSwitch(LogEventLevel.Verbose);

            var log = new LoggerConfiguration()
                .WriteTo.DynamicSwitch(switchName, logSwitch)
                .CreateLogger();

            var provider = new ServiceCollection()
                .AddDynamicSwitches()
                .BuildServiceProvider();

            var collection = provider.GetRequiredService<DynamicSwitchCollection>();

            Assert.Equal(DynamicSwitchCollection.Current, collection);
            Assert.Single(collection);
            Assert.Equal(LogEventLevel.Verbose, collection[switchName].MinimumLevel);
        }
    }
}