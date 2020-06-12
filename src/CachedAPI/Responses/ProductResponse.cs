using System;
using System.Collections.Generic;
using System.Linq;
using CachedAPI.Entities;

namespace CachedAPI.Responses
{
    public class ProductResponse
    {
        public string ResponseTime { get; set; }
        public int Items { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public ProductResponse(IEnumerable<Product> products, DateTime startRequest)
        {
            Products = products;
            Items = products.Count();
            ResponseTime = (DateTime.Now - startRequest).ToString();
        }
    }
}
