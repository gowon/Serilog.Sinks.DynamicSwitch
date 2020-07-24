namespace ConsoleAppSample
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Events;
    using Serilog.Sinks.DynamicSwitch;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(AddConfiguration(new ConfigurationBuilder()).Build())
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var switches = scope.ServiceProvider.GetRequiredService<DynamicSwitchCollection>();

                foreach (var (key, value) in switches)
                {
                    Console.WriteLine($"{key} => {value.MinimumLevel}");
                }

                logger.LogWarning("This will be captured.");
                logger.LogInformation("This will not be captured.");
                switches["Application"].MinimumLevel = LogEventLevel.Information;
                logger.LogInformation("This will be captured now.");
            }
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
                    services.AddDynamicSwitches();
                });
        }
    }
}