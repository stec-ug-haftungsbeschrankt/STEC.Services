using System;
using Stripe;

namespace STEC.Services.Billing
{
    public class BillingProduct
    {
        public string Name { get; set; }

        public int Price { get; set; } // In Cent e.g. 14€ are 1400 cent

        public Currencies Currency { get; set; }

        public bool IsSubscription { get; set; }

        public Subscription Subscription { get; set; }

        public bool HasTrial { get; set; }

        public int TrialDays { get; set; }
    }

}