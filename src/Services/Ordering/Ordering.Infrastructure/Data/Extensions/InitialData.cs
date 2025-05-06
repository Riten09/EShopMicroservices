

namespace Ordering.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<Customer> Customers =>
            new List<Customer>
            {
                Customer.Create(CustomerId.Of(new Guid("b566d21c-0496-4d8c-b597-af4decc36e05")), "Riten","riten@gmail.com"),
                Customer.Create(CustomerId.Of(new Guid("cad821ed-e69e-4ad1-8407-85cff9deb593")), "Ritu","Ritu@gmail.com")
            };

        public static IEnumerable<Product> Products =>
            new List<Product>
            {
                Product.Create(ProductId.Of(new Guid("9a5698f0-c2ff-454f-a0d3-1636d2c03a91")),"Iphone 16 Pro Max", 1000),
                Product.Create(ProductId.Of(new Guid("bdd65744-1795-44a5-a83b-07da8552ad4c")),"Samsung S25 Ultra", 1200),
                Product.Create(ProductId.Of(new Guid("044449dc-af1a-4f97-bb50-d66f72d97218")),"MAC Book Air", 750),
                Product.Create(ProductId.Of(new Guid("eac30ad3-ea45-46b6-a980-698912256fa6")),"Gigabytes RTX 5090", 1900)
            };

        public static IEnumerable<Order> OrderIWithItems
        {
            get
            {
                var address1 = Address.Of("Riten", "Mall", "ritenmall@gmail.com", "Dehradun","India", "Uttrakhand", "24800");
                var address2 = Address.Of("Ritu", "Mall", "ritumall@gmail.com", "Dehradun","India", "Uttrakhand", "24800");

                var payment1 = Payment.Of("VisaCard", "5555555555554444", "12/28", "355", 1);
                var payment2 = Payment.Of("MasterCard", "5555555555554444", "07/28", "467", 2);

                var order1 = Order.Create(
                    OrderId.Of(Guid.NewGuid()),
                    CustomerId.Of(new Guid("b566d21c-0496-4d8c-b597-af4decc36e05")),
                    OrderName.Of("ORD_1"),
                    shippingAddress: address1,
                    billingAddress:address1,
                    payment:payment1
                    );
                order1.Add(ProductId.Of(new Guid("9a5698f0-c2ff-454f-a0d3-1636d2c03a91")), 1, 1000);
                order1.Add(ProductId.Of(new Guid("bdd65744-1795-44a5-a83b-07da8552ad4c")), 2, 2400);

                var order2 = Order.Create(
                    OrderId.Of(Guid.NewGuid()),
                    CustomerId.Of(new Guid("cad821ed-e69e-4ad1-8407-85cff9deb593")),
                    OrderName.Of("ORD_2"),
                    shippingAddress: address2,
                    billingAddress:address2,
                    payment:payment2
                    );
                order2.Add(ProductId.Of(new Guid("044449dc-af1a-4f97-bb50-d66f72d97218")), 2, 1500);
                order2.Add(ProductId.Of(new Guid("eac30ad3-ea45-46b6-a980-698912256fa6")), 1, 1900);

                return new List<Order> { order1, order2 };

            }
        }
            
    }
}
