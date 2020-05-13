namespace Serilog.Sinks.DynamicSwitch.Tests.Support
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Events;
    using Xunit.Sdk;

    [ExcludeFromCodeCoverage]
    public static class LogEventHelper
    {
        public static LogEvent LogEvent(string messageTemplate, params object[] propertyValues)
        {
            return LogEvent(null, messageTemplate, propertyValues);
        }

        public static LogEvent LogEvent(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            return LogEvent(LogEventLevel.Information, exception, messageTemplate, propertyValues);
        }

        public static LogEvent LogEvent(LogEventLevel level, Exception exception, string messageTemplate,
            params object[] propertyValues)
        {
            var log = new LoggerConfiguration().CreateLogger();
            if (!log.BindMessageTemplate(messageTemplate, propertyValues, out var template, out var properties))
            {
                throw new XunitException("Template could not be bound.");
            }

            return new LogEvent(DateTimeOffset.Now, level, exception, template, properties);
        }
    }
}