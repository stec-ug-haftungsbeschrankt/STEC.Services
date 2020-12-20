using System;
using System.Linq;
using System.Threading.Tasks;

namespace STEC.Services.UserTenants
{
    public interface ITenantProvider
    {
        Task<string> GetTenantSchema();

        Task<Tenant> GetTenant();
    }
}