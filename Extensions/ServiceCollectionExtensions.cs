using CoreApiSamples.Core;
using CoreApiSamples.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace CoreApiSamples.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddAndMigrateTenantDatabases<CTX>(this IServiceCollection services, IConfiguration config) where CTX : DbContext
        {
            var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));
            //var defaultConnectionString = options.Defaults?.ConnectionString;
            var defaultDbProvider = options.Defaults?.DBProvider;
            if (defaultDbProvider.ToLower() == "mssql")
            {
                services.AddDbContext<CTX>(m => m.UseSqlServer(e => e.MigrationsAssembly(typeof(CTX).Assembly.FullName))); //ToDo: Better way
            }
            var tenants = options.Tenants;
            foreach (var tenant in tenants)
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CTX>();
                dbContext.Database.SetConnectionString(tenant.ConnectionString); //No need to check if tenant.ConnectionString is null/empty
                if (dbContext.Database.GetMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }
            return services;
        }

        public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services, IConfiguration config)
        {
            var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));
            //var defaultConnectionString = options.Defaults?.ConnectionString;
            var defaultDbProvider = options.Defaults?.DBProvider;
            if (defaultDbProvider.ToLower() == "mssql")
            {
                services.AddDbContext<PatientDbContext>(m => m.UseSqlServer(e => e.MigrationsAssembly(typeof(PatientDbContext).Assembly.FullName))); //ToDo: Better way
            }
            var tenants = options.Tenants;
            foreach (var tenant in tenants)
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<PatientDbContext>();
                dbContext.Database.SetConnectionString(tenant.ConnectionString); //No need to check if tenant.ConnectionString is null/empty
                if (dbContext.Database.GetMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }
            return services;
        }

        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }
    }
}
