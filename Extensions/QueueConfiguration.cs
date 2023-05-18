using CoreApiSamples.Core;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoreApiSamples.Extensions
{
    public static class QueueConfiguration
    {
        public static void ConfigureQueue(this IServiceCollection services, TenantSettings tenantSettings)
        {
            if (!string.IsNullOrEmpty(tenantSettings.Defaults.HangfireConnection))
            {
                services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseFilter(new AutomaticRetryAttribute { Attempts = 0 })
                    .UseSqlServerStorage(tenantSettings.Defaults.HangfireConnection, new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        UsePageLocksOnDequeue = true,
                        DisableGlobalLocks = true
                    }));
            }
            else
            {
                services.AddHangfire(c =>
                {
                    c.UseMemoryStorage();
                    c.UseFilter(new AutomaticRetryAttribute { Attempts = 0 });
                });
                Console.WriteLine("Using inmemory database for queue");
            }

            foreach (var tenant in tenantSettings.Tenants)
            {
                services.AddHangfireServer(options =>
                {
                    options.Queues = new[] { tenant.TenantId };
                    options.WorkerCount = 2;
                });
            }
        }
    }
}