using CoreApiSamples.Core;
using CoreApiSamples.Extensions;
using CoreApiSamples.Repositories;
using CoreApiSamples.Services;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreApiSamples
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly RootOptions _rootOptions;
        private readonly TenantSettings _tenantSettings;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _rootOptions = _configuration.Get<RootOptions>();
            _tenantSettings = _rootOptions.TenantSettings;
            _logger = logger;
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            //services.Configure<TenantSettings>(options => _configuration.Get<TenantSettings>());
            services.AddHttpContextAccessor();
            
            services.AddTransient<ITenantService, TenantService>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IHangfireService, HangfireService>();
            
            services.Configure<TenantSettings>(_configuration.GetSection(nameof(TenantSettings)));

            services.AddAndMigrateTenantDatabases<PatientDbContext>(_configuration);
            services.ConfigureQueue(_tenantSettings);
            services.AddControllers();
            services.AddHostedService<MyBackgroundService>();

            // Register the per-tenant background service
            services.AddSingleton<ITenantBackgroundService, TenantBackgroundService>();
            //services.AddScoped<ITenantBackgroundService, TenantBackgroundService>();

            // Register the background service factory
            services.AddSingleton<ITenantBackgroundServiceFactory, TenantBackgroundServiceFactory>();

            // Register the background service manager
            services.AddSingleton<IHostedService, BackgroundServiceManager>();

            //services.MigrateDbContext<PatientDbContext>(Configuration); //Old
            //services.AddDbContext<PatientDbContext>(SetupDb); //Old
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        {
            if (env.IsDevelopment())
            {
                _logger.LogInformation(@"Env is development.");
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //MigrateDbContext<PatientDbContext>(serviceScope); //Old
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard();
        }

        private void SetupDb(DbContextOptionsBuilder options)
        {
           
        }

        private void MigrateDbContext<T>(IServiceScope serviceScope) where T : DbContext
        {
            var dbContext = serviceScope.ServiceProvider.GetService<T>();
            if (dbContext.Database.IsSqlServer())
            {
                dbContext.Database.Migrate();
            }
        }

    }
}
