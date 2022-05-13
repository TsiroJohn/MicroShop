using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : Controller
    {
        private readonly IDiscountRepository _repository;

        public DiscountController(IDiscountRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{productId}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupons), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupons>> GetDiscount(string productId)
        {
            var Coupons = await _repository.GetDiscount(productId);
            return Ok(Coupons);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupons), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupons>> CreateDiscount([FromBody] Coupons Coupons)
        {
            await _repository.CreateDiscount(Coupons);
            return CreatedAtRoute("GetDiscount", new { productId = Coupons.ProductId }, Coupons);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Coupons), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupons>> UpdateDiscount([FromBody] Coupons Coupons)
        {
            return Ok(await _repository.UpdateDiscount(Coupons));
        }

        [HttpDelete("{productId}", Name = "DeleteDiscount")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteDiscount(string productId)
        {
            return Ok(await _repository.DeleteDiscount(productId));
        }
    }
}
