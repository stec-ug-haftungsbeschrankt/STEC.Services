using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Xunit;

namespace STEC.Services.Billing.Tests
{
    public class ProductBillingTests
    {
        private readonly BillingOptions _billingOptions = new BillingOptions()
        {
            StripeApiKey = "<your_api_key>"
        };
        
        [Fact]
        public void CreateBillingInstance()
        {
            var options = new OptionsWrapper<BillingOptions>(_billingOptions);
            
            var loggerFactory = new LoggerFactory();
            var logger = new Logger<Billing>(loggerFactory);
            
            IBilling billing = new Billing(options, logger);
            Assert.NotNull(billing);
            
            loggerFactory.Dispose();
        }

        [Fact]
        public void CreateFixedPriceProduct()
        {
            var options = new OptionsWrapper<BillingOptions>(_billingOptions);
            
            var loggerFactory = new LoggerFactory();
            var logger = new Logger<Billing>(loggerFactory);
            
            IBilling billing = new Billing(options, logger);
            Assert.NotNull(billing);

            var product = new BillingProduct()
            {
                Name = "Test Fixed Price Product",
                Price = 1530,
                Currency = Currencies.EUR,
                HasTrial = false,
                IsSubscription = false
            };
            var productId = billing.CreateProduct(product);
            
            Assert.False(string.IsNullOrEmpty(productId));
            
            loggerFactory.Dispose();
        }

        [Fact]
        public void CreateSubscriptionProduct()
        {
            var options = new OptionsWrapper<BillingOptions>(_billingOptions);
            
            var loggerFactory = new LoggerFactory();
            var logger = new Logger<Billing>(loggerFactory);
            
            IBilling billing = new Billing(options, logger);
            Assert.NotNull(billing);


            // FIXME
            
            loggerFactory.Dispose();
        }
    }
}