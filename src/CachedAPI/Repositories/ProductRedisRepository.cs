using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Bogus;
using CachedAPI.Constants;
using CachedAPI.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace CachedAPI.Repositories
{
    public class ProductRedisRepository
    {
        private static List<Product> data;
        private readonly Faker faker;
        private readonly IDistributedCache _redisCache;

        public ProductRedisRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;

            if (data != null) return;

            faker = new Faker();
            data = new List<Product>();

            for (int i = 0; i < 5000; i++)
            {
                data.Add(GetRandom());
            }
        }

        private Product GetRandom()
        {
            return new Product(
                id: Guid.NewGuid(),
                productName: $"{faker.Commerce.Product()}: {faker.Commerce.ProductName()}",
                category: faker.Commerce.Categories(1).First(),
                price: Decimal.Parse(faker.Commerce.Price()),
                ean13: faker.Commerce.Ean13()
            );
        }

        private IEnumerable<Product> GetFromCache(string cacheKey)
        {
            var cacheData = _redisCache.GetString(cacheKey);

            if (cacheData != null)
                return JsonSerializer.Deserialize<IEnumerable<Product>>(cacheData);

            return null;
        }

        private DistributedCacheEntryOptions GetCacheOptions(
            int slidingExpirationSecs = 0,
            int absoluteExpirationSecs = 0)
        {
            var cacheOptions = new DistributedCacheEntryOptions();

            if (slidingExpirationSecs > 0)
                cacheOptions.SetSlidingExpiration(TimeSpan.FromSeconds(slidingExpirationSecs)); // inactive

            if (absoluteExpirationSecs > 0)
                cacheOptions.SetAbsoluteExpiration(TimeSpan.FromSeconds(absoluteExpirationSecs)); // absolute

            return cacheOptions;
        }

        public IEnumerable<Product> GetAll()
        {
            IEnumerable<Product> products = GetFromCache(CacheKeys.PRODUCTS_GET_ALL);
            if (products != null) { return products; }

            products = data;

            System.Threading.Thread.Sleep(5000);
            Console.WriteLine($"Created cache entry: {CacheKeys.PRODUCTS_GET_ALL}");

            var toCache = JsonSerializer.Serialize(products);
            _redisCache.SetString(CacheKeys.PRODUCTS_GET_ALL, toCache, GetCacheOptions(15, 30));

            return products;
        }

        public IEnumerable<Product> GetProductsByName(string name)
        {
            IEnumerable<Product> products = GetFromCache($"{CacheKeys.PRODUCTS_GET_BY_NAME}_{name}");
            if (products != null) { return products; }

            products = data.Where(it => it.ProductName.Contains(name, StringComparison.InvariantCultureIgnoreCase));

            System.Threading.Thread.Sleep(3000);
            Console.WriteLine("Created cache entry: Products_GetByName");

            var toCache = JsonSerializer.Serialize(products);
            _redisCache.SetString($"{CacheKeys.PRODUCTS_GET_BY_NAME}_{name}", toCache, GetCacheOptions(10));

            return products;
        }

        public IEnumerable<Product> GetProductsByLowerPrice(decimal price)
        {
            IEnumerable<Product> products = GetFromCache(CacheKeys.PRODUCTS_GET_BY_LOWER_PRICE);
            if (products != null) { return products; }

            products = data.Where(it => it.Price <= price);

            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("Created cache entry: Products_GetByLowerPrice");

            var toCache = JsonSerializer.Serialize(products);
            _redisCache.SetString(CacheKeys.PRODUCTS_GET_BY_LOWER_PRICE, toCache, GetCacheOptions(0, 10));

            return products;
        }

        public IEnumerable<Product> GetProductsByUpperPrice(decimal price)
        {
            IEnumerable<Product> products = GetFromCache(CacheKeys.PRODUCTS_GET_BY_UPPER_PRICE);
            if (products != null) { return products; }

            products = data.Where(it => it.Price >= price);

            System.Threading.Thread.Sleep(2000);
            Console.WriteLine("Created cache entry: Products_GetByUpperPrice");

            var toCache = JsonSerializer.Serialize(products);
            _redisCache.SetString(CacheKeys.PRODUCTS_GET_BY_UPPER_PRICE, toCache, GetCacheOptions(0, 10));

            return products;
        }
    }
}
