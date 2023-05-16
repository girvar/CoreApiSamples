using CoreApiSamples.Core;
using CoreApiSamples.Infra;
using CoreApiSamples.Repositories;
using CoreApiSamples.Services;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddTransient<ITenantService, TenantService>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IPatientService, PatientService>();
            
            services.Configure<TenantSettings>(Configuration.GetSection(nameof(TenantSettings)));
            services.AddAndMigrateTenantDatabases(Configuration);
            //services.AddDbContext<PatientDbContext>(SetupDb);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //MigrateDbContext<PatientDbContext>(serviceScope);
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

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
