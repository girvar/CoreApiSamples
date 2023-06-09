using CoreApiSamples.Core;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CoreApiSamples.Services
{
    public interface IHangfireService
    {
        void CreateRecurringJob();
        void RemoveRecurringJobIfExist(string recurringJobId);
        List<string> GetJobs();
    }

    public class HangfireService : IHangfireService
    {
        private readonly ITenantService _tenantService;
        private readonly ILogger<HangfireService> _logger;
        public HangfireService(ITenantService tenantService, ILogger<HangfireService> logger) 
        {
            _tenantService = tenantService;
            _logger = logger;
        }
        public void CreateRecurringJob()
        {
            string tenantId = _tenantService.GetCurrentTenant().TenantId;
            _logger.LogInformation($"Adding recurring job to tenant -> {tenantId}...");
            RecurringJob.AddOrUpdate(() => Console.WriteLine($"This is per minute recurring Job using hangfire for tenant {tenantId}"), Cron.Minutely, null, tenantId);
        }

        public List<string> GetJobs()
        {
            return new List<string>();
        }

        public void RemoveRecurringJobIfExist(string recurringJobId)
        {
        }
    }
}