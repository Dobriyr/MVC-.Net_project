using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Business.Validation;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
    
        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetAsync()
        {
            var customers = await _customerService.GetAllAsync();
            return customers != null ? Ok(customers) : (ActionResult<IEnumerable<CustomerModel>>)NotFound();
        }

        //GET: api/customers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        //GET: api/customers/products/1
        [HttpGet("products/{id}")]
        public async Task<ActionResult<CustomerModel>> GetByCustomersByProductId(int id)
        {
            try
            {
                var p = await _customerService.GetCustomersByProductIdAsync(id);
                return Ok(p);
            }
            catch (MarketException ex) {
                return NotFound(ex.Message);
            }
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CustomerModel value)
        {
            try
            {
                await _customerService.AddAsync(value);
            }
            catch (MarketException ex) {
                return BadRequest(ex.Message);
            }
            return Ok(value);
        }

        // PUT: api/customers/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] CustomerModel value)
        {
            try
            {
                await _customerService.UpdateAsync(value); 
            }
            catch (MarketException ex) {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        // DELETE: api/customers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _customerService.DeleteAsync(id);
                return Ok();
            }
            catch (MarketException ex) {
                return NotFound(ex.Message);
            }
        }
    }
}
