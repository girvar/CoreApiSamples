using CoreApiSamples.Core;
using Microsoft.EntityFrameworkCore;

namespace CoreApiSamples.Repositories
{
    public abstract class MultitenantDbContext : DbContext
    {
        protected readonly ITenantService TenantContext;

        protected MultitenantDbContext(DbContextOptions<PatientDbContext> options, ITenantService tenantContext) : base(options) 
        {
            TenantContext = tenantContext;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = TenantContext.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = TenantContext.GetDatabaseProvider();
                if (DBProvider.ToLower() == "mssql") //ToDo: Check
                {
                    optionsBuilder.UseSqlServer(TenantContext.GetConnectionString());
                }
            }
        }
    }
}