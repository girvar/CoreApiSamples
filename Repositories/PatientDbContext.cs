using CoreApiSamples.Core;
using CoreApiSamples.Services;
using Microsoft.EntityFrameworkCore;

namespace CoreApiSamples.Repositories
{
    public class PatientDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        public PatientDbContext(DbContextOptions<PatientDbContext> options, ITenantService tenantService)
           : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
            .HasKey(i => i.Identity);

            modelBuilder.Entity<Patient>()
                .HasIndex(i => i.ID1)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = _tenantService.GetDatabaseProvider();
                if (DBProvider.ToLower() == "mssql") //ToDo: Check
                {
                    optionsBuilder.UseSqlServer(_tenantService.GetConnectionString());
                }
            }
        }
    }
}
