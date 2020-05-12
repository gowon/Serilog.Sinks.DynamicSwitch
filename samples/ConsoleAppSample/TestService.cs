namespace ConsoleAppSample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog.Events;
    using Serilog.Sinks.DynamicSwitch;

    public class TestService : BackgroundService
    {
        private readonly ILogger<TestService> _logger;
        private readonly LoggingLevelSwitchCollection _switches;

        public TestService(LoggingLevelSwitchCollection switches, ILogger<TestService> logger)
        {
            _switches = switches ?? throw new ArgumentNullException(nameof(switches));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var pair in _switches)
            {
                Console.WriteLine($"{pair.Key} => {pair.Value}");
            }

            _logger.LogWarning("This will be captured.");

            _logger.LogInformation("This will not be captured.");

            _switches["Application"].MinimumLevel = LogEventLevel.Information;

            _logger.LogInformation("This will be captured now.");

            return Task.CompletedTask;
        }
    }
}