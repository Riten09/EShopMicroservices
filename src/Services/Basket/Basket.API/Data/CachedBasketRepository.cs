
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {
        

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            // adding caching logic to check the data in cache
            var cacheBasket = await cache.GetStringAsync(userName, cancellationToken);
            if(!string.IsNullOrEmpty(cacheBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cacheBasket)!; // returing data from cache

            var basket = await repository.GetBasket(userName, cancellationToken); // getting the data from DB if no data is there is cache
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken); // inserting the data in cache 
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            // storing the basket data in DB 
            await repository.StoreBasket(basket, cancellationToken);

            // updating or inserting the data into cache 
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);  

            return basket;
        }
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            //delete the basket record from DB
            await repository.DeleteBasket(userName, cancellationToken);

            // delete the same from cache 
            await cache.RemoveAsync(userName, cancellationToken);

            return true;
        }
    }
}
