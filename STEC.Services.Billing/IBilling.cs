namespace STEC.Services.Billing
{
    public interface IBilling
    {
        void EnsureProduct(BillingProduct product);
        bool ProductExists(BillingProduct product);

        string CreateProduct(BillingProduct product);
        bool CreatePrice(string productId, BillingProduct product);

        bool UpdateProduct(BillingProduct product);

        bool DeleteProduct(string productId);
    }
}