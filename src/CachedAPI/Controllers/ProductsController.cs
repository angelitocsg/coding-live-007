using System;
using System.Collections.Generic;
using System.Linq;
using CachedAPI.Entities;
using CachedAPI.Repositories;
using CachedAPI.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CachedAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMemoryCache _memCache;

        public ProductsController(ILogger<ProductsController> logger, IMemoryCache memCache)
        {
            _logger = logger;
            _memCache = memCache;
        }

        [HttpGet]
        public ActionResult<ProductResponse> GetAll()
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRepository(_memCache).GetAll(),
                startRequest: start
            );
            return Ok(response);
        }

        [HttpGet("name")]
        public ActionResult<ProductResponse> GetByName([FromQuery] string name)
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRepository(_memCache).GetProductsByName(name),
                startRequest: start
            );
            return Ok(response);
        }

        [HttpGet("lower-price")]
        public ActionResult<ProductResponse> GetByLowerPrice([FromQuery] decimal price)
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRepository(_memCache).GetProductsByLowerPrice(price),
                startRequest: start
            );
            return Ok(response);
        }

        [HttpGet("upper-price")]
        public ActionResult<ProductResponse> GetByUpperPrice([FromQuery] decimal price)
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRepository(_memCache).GetProductsByUpperPrice(price),
                startRequest: start
            );
            return Ok(response);
        }
    }
}
