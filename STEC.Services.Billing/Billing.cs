using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;

namespace STEC.Services.Billing
{
    public class Billing : IBilling
    {
        public Billing(IOptions<BillingOptions> options, ILogger<Billing> logger)
        {
            if (options != null)
            {
                _options = options.Value;
                StripeConfiguration.ApiKey = _options.StripeApiKey;
            }
            _logger = logger;
        }

        private readonly ILogger _logger;

        private readonly BillingOptions _options;

        public string CreatePaymentIntent(IList<BillingProduct> items)
        {
            var paymentIntents = new PaymentIntentService();

            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
            {
                Amount = CalculateOrderAmount(items),
                Currency = "usd",
            });

            return paymentIntent.ClientSecret;
        }


        private int CalculateOrderAmount(IList<BillingProduct> items)
        {
            // Replace this constant with a calculation of the order's amount
            // Calculate the order total on the server to prevent
            // people from directly manipulating the amount on the client
            return 1400;
        }

        public void EnsureProduct(BillingProduct product)
        {
            if (ProductExists(product))
            {
                return;
            }
            var productId = CreateProduct(product);
            CreatePrice(productId, product);
        }

        public bool ProductExists(BillingProduct product)
        {
            var productService = new ProductService();
            var options = new ProductListOptions();
            
            var stripeProducts = productService.List(options);
            var stripeProduct = stripeProducts.SingleOrDefault(x => x.Name == product.Name);

            if (stripeProduct is null)
            {
                return false;
            }
            
            var priceService = new PriceService();
            var priceOptions = new PriceListOptions()
            {
                Product = stripeProduct.Id
            };
            var stripePrices = priceService.List(priceOptions);
            var stripePrice = stripePrices.SingleOrDefault(x =>
                x.UnitAmount == product.Price && 
                x.Currency == product.Currency.ToString().ToLower());

            if (stripePrice is null)
            {
                return false;
            }
            return true;
        }

        public bool CreatePrice(string productId, BillingProduct product)
        {
            var priceOptions = new PriceCreateOptions()
            {
                Product = productId,
                UnitAmount = product.Price,
                Currency = product.Currency.ToString().ToLower()
            };

            if (product.IsSubscription)
            {
                priceOptions.Recurring = new PriceRecurringOptions()
                {
                    Interval = product.Subscription.ToString().ToLower(),
                };

                if (product.HasTrial)
                {
                    priceOptions.Recurring.TrialPeriodDays = product.TrialDays;
                }
            }

            var priceService = new PriceService();
            var stripePrice = priceService.Create(priceOptions);
            return stripePrice is object;
        }

        public string CreateProduct(BillingProduct product)
        {
            var productOptions = new ProductCreateOptions()
            {
                Name = product.Name
            };

            var productService = new ProductService();
            var stripeProduct = productService.Create(productOptions);
            
            return stripeProduct.Id;
        }

        public bool UpdateProduct(BillingProduct product)
        {
            throw new NotImplementedException();
        }



        // Likely broken by design.
        public bool DeleteProduct(string productId)
        {
            var productService = new ProductService();
            var stripeProduct = productService.Get(productId);

            if (stripeProduct is null)
            {
                return false;
            }

            // Delete Prices
            var priceService = new PriceService();
            var priceListOptions = new PriceListOptions()
            {
                Product = productId
            };
            var stripePriceList = priceService.List(priceListOptions);

            foreach (var stripePrice in stripePriceList)
            {
                // We cannot delete prices
            }

            var deletedProduct = productService.Delete(productId);

            if (deletedProduct is null)
            {
                return false;
            }
            return true;
        }
    }
}
