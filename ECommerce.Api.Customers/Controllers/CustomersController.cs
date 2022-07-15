using ECommerce.Api.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersProvider customerProvider;

        public CustomersController(ICustomersProvider customerProvider)
        {
            this.customerProvider = customerProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var result = await customerProvider.GetCustomersAsync();

            if (result.IsSuccess)
            {
                return Ok(result.products);
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var result = await customerProvider.GetCustomerAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.product);
            }

            return NotFound();
        }
    }
}
