using System;
using System.Collections.Generic;
using System.Linq;
using CachedAPI.Entities;
using CachedAPI.Repositories;
using CachedAPI.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CachedAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<ProductResponse> GetAll()
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRepository().GetAll(),
                startRequest: start
            );
            return Ok(response);
        }

        [HttpGet("name")]
        public ActionResult<ProductResponse> GetByName([FromQuery] string name)
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRepository().GetProductsByName(name),
                startRequest: start
            );
            return Ok(response);
        }

        [HttpGet("lower-price")]
        public ActionResult<ProductResponse> GetByLowerPrice([FromQuery] decimal price)
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRepository().GetProductsByLowerPrice(price),
                startRequest: start
            );
            return Ok(response);
        }

        [HttpGet("upper-price")]
        public ActionResult<ProductResponse> GetByUpperPrice([FromQuery] decimal price)
        {
            var start = DateTime.Now;
            var response = new ProductResponse(
                products: new ProductRepository().GetProductsByUpperPrice(price),
                startRequest: start
            );
            return Ok(response);
        }
    }
}
