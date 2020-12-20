using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace STEC.Services.UserTenants
{
    public class PostgreSqlTenantProvider : ITenantProvider
    {
        private readonly TenantDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PostgreSqlTenantProvider> _logger;

        public PostgreSqlTenantProvider(IHttpContextAccessor accessor, UserManager<User> userManager, TenantDbContext context, ILogger<PostgreSqlTenantProvider> logger)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<string> GetTenantSchema()
        {
            var tenant = await GetTenant().ConfigureAwait(false);

            if (tenant == null)
            {
                return "public";
            }

            if (tenant.Type == TenantType.TablePrefix)
            {
                return tenant.TablePrefix.ToString();
            }
            return "public";
        }

        public async Task<Tenant> GetTenant()
        {
            if (_accessor.HttpContext == null)
            {
                return null;
            }

            var rawUser = await _userManager.GetUserAsync(_accessor.HttpContext?.User).ConfigureAwait(false);
            // Reload User to contain Tenant. GetUserAsync does not resolve this one
            var user = _context.Users.Include(u => u.Tenant).SingleOrDefault(u => u.Id == rawUser.Id);

            if (user == null)
            {
                _logger.LogInformation("User not found");
                return null;
            }

            if (user.Tenant == null)
            {
                _logger.LogInformation($"Tenant for User {user.UserName} not found");
                return null;
            }

            var tenant = _context.Tenants.Include(t => t.Users)
                                         .SingleOrDefault(t => t.ID == user.Tenant.ID);

            if (tenant == null)
            {
                // FIXME Create new Tenant???
                return null;
            }
            return tenant;
        }
    }
}
