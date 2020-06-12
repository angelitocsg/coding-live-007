using System;

namespace CachedAPI.Entities
{
    public class Product
    {
        private Product() { }

        public Product(Guid id, string productName, string category, decimal price, string ean13)
        {
            Id = id;
            ProductName = productName;
            Category = category;
            Price = price;
            Ean13 = ean13;
        }

        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Ean13 { get; set; }
    }
}
