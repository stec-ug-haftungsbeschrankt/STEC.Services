using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace STEC.Services.UserTenants
{
    public class ApplicationDbContextBase : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

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
            if (optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));

            // Used for Schema Switch
            optionsBuilder.ReplaceService<IModelCacheKeyFactory, ApplicationCacheKeyFactory>();
            // Used for Migration schema awareness
            optionsBuilder.ReplaceService<IMigrationsAssembly, DbSchemaAwareMigrationAssembly>();

            // Make sure, every Tenant has it's own MigrationHistory table
            var builder = new NpgsqlDbContextOptionsBuilder(optionsBuilder);
            builder.MigrationsHistoryTable("__EFMigrationsHistory", Schema);

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
