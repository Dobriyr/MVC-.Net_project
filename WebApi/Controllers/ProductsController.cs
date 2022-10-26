using Business.Interfaces;
using Business.Models;
using Business.Services;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;


        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetWithFilter([FromQuery] FilterSearchModel filter)
        {
            var filteredProducts = await _productService.GetByFilterAsync(filter);
            return Ok(filteredProducts);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProductById(int id)
        {
            try
            {
                var products = await _productService.GetByIdAsync(id);
                return Ok(products);
            }
            catch (MarketException ex) {
                return NotFound(ex.Message);
            }
        }
        

        [HttpPost]

        public async Task<ActionResult> AddProduct([FromBody] ProductModel product) {
            try
            {
                await _productService.AddAsync(product);
                return Ok(product);
            }
            catch (MarketException ex) {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<int>> UpdateProduct([FromBody] ProductModel model, int id) {
            try { await _productService.UpdateAsync(model); }
            catch (MarketException ex) {
                return BadRequest(ex.Message);
            }
            return Ok(id);
        }
        [HttpDelete]
        [Route("{id}")]

        public async Task<ActionResult> DeleteProduct(int id) {
            try
            {
                await _productService.DeleteAsync(id);
                return Ok();
            }
            catch (MarketException ex) {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetAllCategories()
        {
            try
            {
                var products = await _productService.GetAllProductCategoriesAsync();
                return Ok(products);
            }
            catch (MarketException ex) { return BadRequest(); }
        }
        [HttpPost]
        [Route("categories")]
        public async Task<ActionResult<ProductCategoryModel>> AddCategory([FromBody] ProductCategoryModel model)
        {
            try {
                await _productService.AddCategoryAsync(model);
                return Ok(model);
            }
            catch (MarketException ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("categories/{id}")]
        public async Task<ActionResult<int>> UpdateCategory([FromBody] ProductCategoryModel model, int id)
        {
            try {
                await _productService.UpdateCategoryAsync(model);
                return Ok(id);
            }
            catch (MarketException ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("categories/{id}")]
        public async Task<ActionResult<int>> DeleteCategory(int id)
        {
            try
            {
                await _productService.RemoveCategoryAsync(id);
                return Ok(id);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
