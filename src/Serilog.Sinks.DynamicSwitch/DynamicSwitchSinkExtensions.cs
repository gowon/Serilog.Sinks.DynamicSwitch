namespace Serilog.Sinks.DynamicSwitch
{
    using System;
    using Configuration;
    using Core;
    using Events;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class DynamicSwitchSinkExtensions
    {
        // https://github.com/serilog/serilog-settings-configuration/blob/9f2fb303987d311a0bdaf7a91f25339a0c113f8f/test/TestDummies/DummyLoggerConfigurationExtensions.cs#L101
        public static LoggerConfiguration DynamicSwitch(
            this LoggerSinkConfiguration loggerSinkConfiguration,
            string switchName,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch controlLevelSwitch = null)
        {
            var sink = new DynamicSwitchSink(switchName, controlLevelSwitch);
            var collection = LoggingLevelSwitchCollection.Current;

            if (!collection.TryAdd(sink.Name, sink.Switch))
            {
                throw new ArgumentException($"Key '{sink.Name}' already exists.");
            }

            return loggerSinkConfiguration.Sink(sink, restrictedToMinimumLevel);
        }

        public static IServiceCollection AddSerilogDynamicSwitch(this IServiceCollection services)
        {
            services.TryAddSingleton(LoggingLevelSwitchCollection.Current);
            return services;
        }
    }
}