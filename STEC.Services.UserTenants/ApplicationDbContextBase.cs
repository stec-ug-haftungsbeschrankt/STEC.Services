using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

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

        public ApplicationDbContextBase(DbContextOptions<ApplicationDbContextBase> options, ITenantProvider tenantProvider) : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Used for Schema Switch
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, ApplicationCacheKeyFactory>();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
