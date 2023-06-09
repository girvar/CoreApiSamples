using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using CoreApiSamples.Core;

namespace CoreApiSamples.Services
{
    public class BackgroundServiceManager : IHostedService
    {
        private readonly ITenantService _tenantContext;
        private readonly ITenantBackgroundServiceFactory _backgroundServiceFactory;
        private readonly IList<ITenantBackgroundService> _tenantBackgroundServices;

        public BackgroundServiceManager(ITenantService tenantService, ITenantBackgroundServiceFactory backgroundServiceFactory)
        {
            _tenantContext = tenantService;
            _backgroundServiceFactory = backgroundServiceFactory;
            _tenantBackgroundServices = new List<ITenantBackgroundService>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var tenants = _tenantContext.TenantSettings?.Tenants;

            foreach (var tenant in tenants)
            {
                var backgroundService = _backgroundServiceFactory.Create(tenant);
                _tenantBackgroundServices.Add(backgroundService);
                _ = backgroundService.StartAsync(cancellationToken);
                await Task.Delay(0, cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var tenantBackgroundService in _tenantBackgroundServices)
            {
                await tenantBackgroundService.StopAsync(cancellationToken);
            }
        }
    }
}
