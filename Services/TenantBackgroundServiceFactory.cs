using CoreApiSamples.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CoreApiSamples.Services
{
    public interface ITenantBackgroundServiceFactory
    {
        ITenantBackgroundService Create(Tenant tenant);
    }

    public class TenantBackgroundServiceFactory : ITenantBackgroundServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public TenantBackgroundServiceFactory(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceProvider = serviceProvider;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public ITenantBackgroundService Create(Tenant tenant)
        {
            var scopedServiceProvider = _serviceProvider.CreateScope().ServiceProvider;
            return new TenantBackgroundService(_serviceScopeFactory, scopedServiceProvider.GetService<ILogger<TenantBackgroundService>>(), tenant);
        }
    }
}