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
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;


        public ReceiptsController(IReceiptService receiptService)
        {
            this._receiptService = receiptService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetAll()
        {
                var receipts = await this._receiptService.GetAllAsync();
                return Ok(receipts); 
        }

        [HttpGet]
        [Route("{id}")]

        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetById(int id) {
            try
            {
                var receipt = await _receiptService.GetByIdAsync(id);
                return Ok(receipt);
            }
            catch (MarketException ex) {
                return NotFound(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id}/details")]

        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetByIdWithDetails(int id)
        {
            try
            {
                var receipt = await _receiptService.GetReceiptDetailsAsync(id);
                return Ok(receipt);
            }
            catch (MarketException ex) {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}/sum")]
        public async Task<ActionResult<decimal>> getByIdSum(int id) {
            try
            {
                var sum = await _receiptService.ToPayAsync(id);
                return Ok(sum);
            }
            catch (Exception ex) {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("period")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetByPeriod(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var receipts = await _receiptService.GetReceiptsByPeriodAsync(startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
                return Ok(receipts);
            }
            catch (Exception ex) {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddReceipt([FromBody] ReceiptModel receiptModel) {
            try { await _receiptService.AddAsync(receiptModel); }
            catch
            {
                return BadRequest();
            }
            return Ok(receiptModel);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ReceiptModel>> UpdateReceipt([FromBody] ReceiptModel model, int id) {
            try
            {
                await _receiptService.UpdateAsync(model);
                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("{id}/products/add/{productId}/{quantity}")]
        
        public async Task<ActionResult> AddProductsToReceipt(int id, int productId, int quantity) {
            try
            {
                await _receiptService.AddProductAsync(productId, id, quantity);
            }
            catch (MarketException ex) {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        [HttpPut]
        [Route("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> DeleteProductsFromReceipt(int id, int productId, int quantity)
        {
            try
            {
                await _receiptService.RemoveProductAsync(productId, id, quantity);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(id);
        }
        [HttpPut]
        [Route("{id}/checkout")]
        public async Task<ActionResult> CheckOutReceipt(int id) {
            try
            {
                await _receiptService.CheckOutAsync(id);
                return Ok();
            }
            catch (MarketException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteReceipt(int id)
        {
            try
            {
                await _receiptService.DeleteAsync(id);
                return Ok();
            }
            catch (MarketException ex) {
                return BadRequest(ex.Message);
            }
        }

    }
}
