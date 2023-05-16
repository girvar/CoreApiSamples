namespace CoreApiSamples.Core
{
    public interface ITenantService
    {
        public string GetDatabaseProvider();

        public string GetConnectionString();

        public Tenant GetTenant();
    }
}
