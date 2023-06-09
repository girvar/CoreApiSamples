namespace CoreApiSamples.Core
{
    public interface ITenantService
    {
        public TenantSettings TenantSettings { get; }

        public string GetDatabaseProvider();

        public string GetConnectionString();

        public Tenant GetCurrentTenant();

        
        
    }
}
