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
using System;

namespace CoreApiSamples
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly TenantSettings _tenantSettings;
        private readonly RootOptions _rootOptions;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _rootOptions = _configuration.Get<RootOptions>();
            _tenantSettings = _rootOptions.TenantSettings;
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            //services.Configure<TenantSettings>(options => _configuration.Get<TenantSettings>());
            services.AddHttpContextAccessor();
            
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddScoped<IHangfireService, HangfireService>();
            
            services.Configure<TenantSettings>(_configuration.GetSection(nameof(TenantSettings)));

            services.AddAndMigrateTenantDatabases<PatientDbContext>(_configuration);
            services.ConfigureQueue(_tenantSettings);
            services.AddControllers();
            //services.MigrateDbContext<PatientDbContext>(Configuration); //Old
            //services.AddDbContext<PatientDbContext>(SetupDb); //Old
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        {
            if (env.IsDevelopment())
            {
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
