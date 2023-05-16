using CoreApiSamples.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace CoreApiSamples.Services
{
    public class TenantService : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private readonly HttpContext _httpContext;
        private Tenant _currentTenant;

        public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContext = contextAccessor.HttpContext;
            if (_httpContext != null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetTenant(tenantId);
                }
                else
                {
                    throw new Exception("Invalid Tenant!");
                }
            }
        }

        private void SetTenant(string tenantId)
        {
            _currentTenant = _tenantSettings.Tenants.Where(a => a.TenantId == tenantId).FirstOrDefault();
            if (_currentTenant == null || string.IsNullOrEmpty(_currentTenant?.ConnectionString))
            {
                throw new Exception("Invalid Tenant or no connection string specified!");
            }
        }


        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }

        public string GetDatabaseProvider()
        {
            return _tenantSettings.Defaults?.DBProvider;
        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }
    }
}
