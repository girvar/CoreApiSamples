using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApiSamples.Services
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<MyBackgroundService> _logger;

        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background service is running.");
                DisplayQueueInfo();

                // Perform your background tasks here

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }

        public void DisplayQueueInfo()
        {
            // Get an instance of the IMonitoringApi
            var monitoringApi = JobStorage.Current.GetMonitoringApi();

            // Get the list of queues
            var queues = monitoringApi.Queues();


            var names = from queue in queues select queue.Name;

            _logger.LogInformation($"Queue names -> {string.Join(',', names)}" );
        }
    }
}