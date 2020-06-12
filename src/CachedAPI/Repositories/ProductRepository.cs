using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using CachedAPI.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace CachedAPI.Repositories
{
    public class ProductRepository
    {
        private static List<Product> data;
        private readonly Faker faker;
        private readonly IMemoryCache _memCache;

        public ProductRepository(IMemoryCache memCache)
        {
            _memCache = memCache;

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

        public IEnumerable<Product> GetAll()
        {
            var products = _memCache.GetOrCreate("Products_GetAll", entry =>
            {
                Console.WriteLine("Created cache entry: Products_GetAll");

                entry.SlidingExpiration = TimeSpan.FromSeconds(15); // inactive
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30); // absolute
                entry.SetPriority(CacheItemPriority.High); // priority

                System.Threading.Thread.Sleep(5000);
                return data;
            });

            return products;
        }

        public IEnumerable<Product> GetProductsByName(string name)
        {
            var products = _memCache.GetOrCreate("Products_GetByName", entry =>
            {
                Console.WriteLine("Created cache entry: Products_GetByName");

                entry.SlidingExpiration = TimeSpan.FromSeconds(10); // inactive
                entry.SetPriority(CacheItemPriority.High); // priority

                System.Threading.Thread.Sleep(3000);
                return data.Where(it => it.ProductName.Contains(name, StringComparison.InvariantCultureIgnoreCase));
            });

            return products;
        }

        public IEnumerable<Product> GetProductsByLowerPrice(decimal price)
        {
            var products = _memCache.GetOrCreate("Products_GetByLowerPrice", entry =>
            {
                Console.WriteLine("Created cache entry: Products_GetByLowerPrice");

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10); // absolute
                entry.SetPriority(CacheItemPriority.Low); // priority

                System.Threading.Thread.Sleep(2000);
                return data.Where(it => it.Price <= price);
            });

            return products;
        }

        public IEnumerable<Product> GetProductsByUpperPrice(decimal price)
        {
            var products = _memCache.GetOrCreate("Products_GetByUpperPrice", entry =>
            {
                Console.WriteLine("Created cache entry: Products_GetByUpperPrice");

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10); // absolute
                entry.SetPriority(CacheItemPriority.Low); // priority

                System.Threading.Thread.Sleep(2000);
                return data.Where(it => it.Price >= price);
            });

            return products;
        }
    }
}
