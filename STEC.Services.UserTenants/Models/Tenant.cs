using System;
using System.Collections.Generic;

namespace STEC.Services.UserTenants
{
    public enum TenantType
    {
        OwnDatabase,
        TablePrefix
    }


    public class Tenant
    {
        public int ID { get; set; }

        public string Name { get; set; }



        // FIXME Do I need a TenantType? Probably for flexibility
        public TenantType Type { get; set; }

        public Guid TablePrefix { get; set; }

        public string ConnectionString { get; set; }




        public virtual IList<User> Users { get; set; }

        public virtual IList<Service> Services { get; set; }

        public virtual IList<Subscription> Subscriptions { get; set; }
    }
}