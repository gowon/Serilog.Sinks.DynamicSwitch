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
        public static LoggerConfiguration DynamicSwitch(
            this LoggerSinkConfiguration configuration,
            string switchName,
            LoggingLevelSwitch levelSwitch,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            levelSwitch = levelSwitch ?? throw new ArgumentNullException(nameof(levelSwitch));
            var collection = LoggingLevelSwitchCollection.Current;

            if (!collection.TryAdd(switchName, levelSwitch))
            {
                throw new ArgumentException($"Key '{switchName}' already exists.");
            }

            return configuration.Sink(new DynamicSwitchSink(switchName), restrictedToMinimumLevel, levelSwitch);
        }

        public static IServiceCollection AddLoggingLevelSwitchCollection(this IServiceCollection services,
            LoggingLevelSwitchCollection collection = null)
        {
            collection ??= LoggingLevelSwitchCollection.Current;
            services.TryAddSingleton(collection);
            return services;
        }
    }
}