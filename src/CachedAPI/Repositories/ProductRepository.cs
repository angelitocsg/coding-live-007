using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using CachedAPI.Entities;

namespace CachedAPI.Repositories
{
    public class ProductRepository
    {
        private static List<Product> data;
        private readonly Faker faker;

        public ProductRepository()
        {
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
            System.Threading.Thread.Sleep(5000);
            return data;
        }

        public IEnumerable<Product> GetProductsByName(string name)
        {
            System.Threading.Thread.Sleep(3000);
            return data.Where(it => it.ProductName.Contains(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<Product> GetProductsByLowerPrice(decimal price)
        {
            System.Threading.Thread.Sleep(2000);
            return data.Where(it => it.Price <= price);
        }

        public IEnumerable<Product> GetProductsByUpperPrice(decimal price)
        {
            System.Threading.Thread.Sleep(2000);
            return data.Where(it => it.Price >= price);
        }
    }
}
