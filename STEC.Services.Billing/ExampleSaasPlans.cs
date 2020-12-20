using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace STEC.Services.Billing
{
    public class ExampleSaasPlans
    {
        private readonly BillingOptions _billingOptions = new BillingOptions()
        {
            StripeApiKey = "<your_api_key>"
        };

        public void CreateShopPlans()
        {
            var options = new OptionsWrapper<BillingOptions>(_billingOptions);

            var loggerFactory = new LoggerFactory();
            var logger = new Logger<Billing>(loggerFactory);

            IBilling billing = new Billing(options, logger);

            var singleUserMonthlyProduct = new BillingProduct()
            {
                Currency = Currencies.EUR,
                HasTrial = true,
                IsSubscription = true,
                Name = "ShopSingleUserMonthly",
                Price = 3000,
                Subscription = Subscription.Monthly,
                TrialDays = 30
            };
            billing.EnsureProduct(singleUserMonthlyProduct);

            var singleUserYearlyProduct = new BillingProduct()
            {
                Currency = Currencies.EUR,
                HasTrial = true,
                IsSubscription = true,
                Name = "ShopSingleUserYearly",
                Price = 36000,
                Subscription = Subscription.Yearly,
                TrialDays = 30
            };
            billing.EnsureProduct(singleUserYearlyProduct);

            loggerFactory.Dispose();
        }
    }
}
