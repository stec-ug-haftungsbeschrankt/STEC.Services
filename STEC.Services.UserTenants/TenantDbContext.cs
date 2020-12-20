using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace STEC.Services.UserTenants
{
    public class TenantDbContext : IdentityDbContext<User>
    {


        public TenantDbContext(DbContextOptions<TenantDbContext> options)
            : base(options)
        {

        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}

