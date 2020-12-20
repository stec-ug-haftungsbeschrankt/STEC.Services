using System;
using System.Collections.Generic;

namespace STEC.Services.UserTenants
{
    public class PricingInformation
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public virtual IList<Limit> Limits { get; set; }
    }
}