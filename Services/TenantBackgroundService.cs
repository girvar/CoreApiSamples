using CoreApiSamples.Core;
using CoreApiSamples.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApiSamples.Services
{
    public interface ITenantBackgroundService
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
    public class TenantBackgroundService : ITenantBackgroundService
    {
        private readonly ILogger<TenantBackgroundService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Tenant _tenant;
        private int _counter = 0;

        public TenantBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<TenantBackgroundService> logger, Tenant tenant)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _tenant = tenant;
            _counter = 0;
        }

        internal async Task ProcessEvents(CancellationToken cancellationToken)
        {
            while (true)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var patientService = (IPatientService)scope.ServiceProvider.GetService(typeof(IPatientService));

                var dbContext = scope.ServiceProvider.GetRequiredService<PatientDbContext>();
                dbContext.Database.SetConnectionString(_tenant.ConnectionString);

                var patientCount = patientService.GetPatientsCount();

                _logger.LogInformation($"Patients in tenant {_tenant.Name} = {patientCount}, BackgroundTaskCounter = {++_counter}");
                await Task.Delay(5000, cancellationToken);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting background service for tenant: {_tenant.Name}");

            // Start your background tasks for the specific tenant here
            await ProcessEvents(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop and clean up your background tasks for the specific tenant here

            _logger.LogInformation($"Stopping background service for tenant: {_tenant.Name}");
            return Task.CompletedTask;
        }
    }
}
