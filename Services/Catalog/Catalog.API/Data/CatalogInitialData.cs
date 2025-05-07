using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();

            if(await session.Query<Product>().AnyAsync())  //checking if any data is DocumentAlreadyExistsException exist in DB then return 
            {
                return;
            }

            // Marten UPSERT will cater for existing records
            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>()
        {
            new Product
            {
                Id= new Guid("b34fc803-3b2a-4a20-9493-5b40a7b6e527"),
                Name = "IPhone X",
                Category = new List<string> {"Smart Phone"},
                Description = "The phone is manufactured by Apple",
                ImageFile = "product-1.png",
                Price = 950.00M
            },
            new Product
            {
                Id= new Guid("0fb88a64-3cb3-4aea-a6da-80e8b60d6894"),
                Name = "Samsung 24 Ultra",
                Category = new List<string> {"Smart Phone"},
                Description = "The phone is manufactured by Samsung",
                ImageFile = "product-2.png",
                Price = 999.00M
            },
            new Product
            {
                Id= new Guid("cf187445-df37-4e16-acb6-8cd9dfb58d96"),
                Name = "Huawei Plus",
                Category = new List<string> {"Smart Phone"},
                Description = "The phone is manufactured by Huawei",
                ImageFile = "product-3.png",
                Price = 650.00M
            },
            new Product
            {
                Id= new Guid("f469f704-0afd-4397-8561-33df8c7de1b4"),
                Name = "Sony Cyber shot",
                Category = new List<string> {"Camera"},
                Description = "The Camera is manufactured by Sony",
                ImageFile = "product-4.png",
                Price = 350.00M
            },
            new Product
            {
                Id= new Guid("f3a8a6f4-d350-4185-8001-05d813f3775e"),
                Name = "Panasonic Lumix",
                Category = new List<string> {"Camera"},
                Description = "The Camera is manufactured by Panasonic",
                ImageFile = "product-5.png",
                Price = 240.00M
            },
        new Product
            {
                Id= new Guid("84ee3f87-e55e-4cc7-bfbd-3d13d1afe711"),
                Name = "LG G7 ThinQ",
                Category = new List<string> {"Home Kitchem"},
                Description = "The product is manufactured by Lg",
                ImageFile = "product-5.png",
                Price = 240.00M
            }

        };

    }
}
