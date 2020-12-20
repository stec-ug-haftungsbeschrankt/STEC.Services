using System;
using System.Collections.Generic;

namespace STEC.Services.UserTenants
{
    public enum BillingProvider
    {
        Stripe
    }

    public enum BillingPeriod
    {
        Monthly,
        Yearly
    }

    public class Subscription
    {
        public int ID { get; set; }

        public BillingProvider Provider { get; set; }

        public BillingPeriod Period { get; set; }

        public DateTime BillingDate { get; set; }

        public PricingInformation Pricing { get; set; }
    }
}