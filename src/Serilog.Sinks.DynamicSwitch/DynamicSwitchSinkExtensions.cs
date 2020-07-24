namespace Serilog.Sinks.DynamicSwitch
{
    using System;
    using Configuration;
    using Core;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class DynamicSwitchSinkExtensions
    {
        public static LoggerConfiguration DynamicSwitch(
            this LoggerSinkConfiguration configuration,
            string switchName,
            LoggingLevelSwitch levelSwitch)
        {
            return DynamicSwitch(configuration, null, switchName, levelSwitch);
        }

        public static LoggerConfiguration DynamicSwitch(
            this LoggerSinkConfiguration configuration,
            DynamicSwitchCollection collection,
            string switchName,
            LoggingLevelSwitch levelSwitch)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            switchName = !string.IsNullOrWhiteSpace(switchName)
                ? switchName
                : throw new ArgumentNullException(nameof(switchName));

            levelSwitch = levelSwitch ?? throw new ArgumentNullException(nameof(levelSwitch));
            collection ??= DynamicSwitchCollection.Current;

            if (!collection.TryAdd(switchName, levelSwitch))
            {
                throw new ArgumentException($"Key '{switchName}' already exists.");
            }

            return configuration.Logger(_ => { });
        }

        public static IServiceCollection AddDynamicSwitches(this IServiceCollection services,
            DynamicSwitchCollection collection = null)
        {
            collection ??= DynamicSwitchCollection.Current;
            services.TryAddSingleton(collection);
            return services;
        }
    }
}