using System;
using System.Collections.Generic;

namespace STEC.Services.UserTenants
{
    public enum ServiceType
    {
        Commerce,
        Healthcheck,
        TaskManagement,
        Verein,
    }


    public class Service
    {
        public int ID { get; set; }

        public ServiceType Type { get; set; }

        public string Title { get; set; }

        public Uri Url { get; set; }



        public virtual IList<PricingInformation> PricingPlan { get; set; }

        public virtual IList<UserRole> UserRoles { get; set; }
    }




}
