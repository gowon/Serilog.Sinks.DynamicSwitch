namespace ConsoleAppSample
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Sinks.DynamicSwitch;

    internal class Program
    {
        private static void Main(string[] args)
        {
            //var logLevel = new LoggingLevelSwitch(LogEventLevel.Warning);

            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console(levelSwitch: logLevel)
            //    .WriteTo.DynamicSwitch("Test", logLevel)
            //    .CreateLogger();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(AddConfiguration(new ConfigurationBuilder()).Build())
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IConfigurationBuilder AddConfiguration(IConfigurationBuilder builder)
        {
            return builder
                .AddJsonFile("appsettings.json", false, false);
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder => AddConfiguration(builder))
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders();
                    builder.AddSerilog();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLoggingLevelSwitchCollection();
                    services.AddHostedService<TestService>();
                });
        }
    }
}