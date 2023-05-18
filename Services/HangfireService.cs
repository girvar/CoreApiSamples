using CoreApiSamples.Core;
using Hangfire;
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
        public HangfireService(ITenantService tenantService) 
        {
            _tenantService = tenantService;
        }
        public void CreateRecurringJob()
        {
            string tenantId = _tenantService.GetTenant().TenantId;
            Console.WriteLine($"Adding recurring job to tenant -> {tenantId}...");
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