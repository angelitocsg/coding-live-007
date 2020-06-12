using System;
using System.Collections.Generic;
using System.Linq;
using CachedAPI.Entities;
using CachedAPI.Repositories;
using CachedAPI.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CachedAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsRedisController : ControllerBase
    {
        private readonly ILogger<ProductsRedisController> _logger;
        private readonly IDistributedCache _redisCache;

        public ProductsRedisController(ILogger<ProductsRedisController> logger, IDistributedCache redisCache)
        {
            _logger = logger;
            _redisCache = redisCache;
        }

        [HttpGet]
        public ActionResult<ProductResponse> GetAll()
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRedisRepository(_redisCache).GetAll(),
                startRequest: start
            );
            return Ok(response);
        }

        [HttpGet("name")]
        public ActionResult<ProductResponse> GetByName([FromQuery] string name)
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRedisRepository(_redisCache).GetProductsByName(name),
                startRequest: start
            );
            return Ok(response);
        }

        [HttpGet("lower-price")]
        public ActionResult<ProductResponse> GetByLowerPrice([FromQuery] decimal price)
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRedisRepository(_redisCache).GetProductsByLowerPrice(price),
                startRequest: start
            );
            return Ok(response);
        }

        [HttpGet("upper-price")]
        public ActionResult<ProductResponse> GetByUpperPrice([FromQuery] decimal price)
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRedisRepository(_redisCache).GetProductsByUpperPrice(price),
                startRequest: start
            );
            return Ok(response);
        }
    }
}
