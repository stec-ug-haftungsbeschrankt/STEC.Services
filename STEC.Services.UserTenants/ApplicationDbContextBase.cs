using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace STEC.Services.UserTenants
{
    public class ApplicationDbContextBase : DbContext
    {
        protected ITenantProvider _tenantProvider;

        public string Schema
        {
            get
            {
                return _tenantProvider.GetTenantSchema().Result;
            }
        }

        public ApplicationDbContextBase(DbContextOptions options, ITenantProvider tenantProvider) : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Used for Schema Switch
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, ApplicationCacheKeyFactory>();
            base.OnConfiguring(optionsBuilder);
        }

        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            var schema = await _tenantProvider.GetTenantSchema().ConfigureAwait(false);
            modelBuilder.HasDefaultSchema(schema);
            base.OnModelCreating(modelBuilder);
        }

        public async Task<Tenant> GetTenant()
        {
            return await _tenantProvider.GetTenant().ConfigureAwait(false);
        }

        public async Task<IList<User>> GetTenantUsers()
        {
            var tenant = await _tenantProvider.GetTenant().ConfigureAwait(false);
            return tenant.Users;
        }

    }
}
