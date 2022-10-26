using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Business.Validation;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController: ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            this._statisticService = statisticService;
        }
        [HttpGet]
        [Route("customer/{id}/{productCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetConcretPopularProductsOfCustomer(int id, int productCount) {
            try
            {
                var products = await _statisticService.GetCustomersMostPopularProductsAsync(productCount, id);
                return Ok(products);
            }
            catch (MarketException ex) {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("popularProducts")]

        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProduct([FromQuery]int? productCount) {

            try
            {
                var products = await _statisticService.GetMostPopularProductsAsync(productCount ?? 1);
                return Ok(products);
            }
            catch (MarketException ex) {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("activity/{customerCount}")]

        public async Task<ActionResult<IEnumerable<CustomerActivityModel>>> GetMostActiveCustomer(int customerCount, [FromQuery]DateTime? startDate, DateTime? endDate) {
            try
            {
                var customers = await _statisticService
                    .GetMostValuableCustomersAsync(customerCount, startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
                return Ok(customers);
            }
            catch (MarketException ex) {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("income/{categoryId}")]

        public async Task<ActionResult<IEnumerable<CustomerActivityModel>>> GetMostIcomeCategory(int categoryId, [FromQuery] DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var sum = await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
                return Ok(sum);
            }
            catch (MarketException ex) {
                return NotFound(ex.Message);
            }
        }
    }
}
