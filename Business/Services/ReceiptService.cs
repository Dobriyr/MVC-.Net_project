using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Data.Entities;
using Business.Validation;
using System.Linq;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    { 
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public ReceiptService(IUnitOfWork uof, IMapper mapper)
        {
            this._unitOfWork = uof;
            this._mapper = mapper;
        }


        public async Task AddAsync(ReceiptModel model)
        {    
            var receipt =  _mapper.Map<ReceiptModel,Receipt>(model);
            await _unitOfWork.ReceiptRepository.AddAsync(receipt);
            await _unitOfWork.SaveAsync();

        }

      

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)
                          ?? throw new MarketException($"Receipt with Id = {receiptId} does not exists");
            
            if (_unitOfWork.ProductRepository == null && receipt.ReceiptDetails.Any(rd => rd.ProductId == productId))
            {
                receipt.ReceiptDetails.FirstOrDefault(rd => rd.ProductId == productId).Quantity += quantity;
            }
            else
            {

                var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

                if (product == null)
                {
                    throw new MarketException($"Product with Id = {productId} does not exists");
                }
                var entity = new ReceiptDetail()
                {
                    ProductId = productId,
                    ReceiptId = receiptId,
                    Quantity = quantity,
                    UnitPrice = product.Price,

                    DiscountUnitPrice = product.Price * (1 - receipt.Customer.DiscountValue / 100m)
                };


                await _unitOfWork.ReceiptDetailRepository.AddAsync(entity);
            }

            await _unitOfWork.SaveAsync();
        }
        public async Task CheckOutAsync(int receiptId)
        {
           var receipt = await _unitOfWork.ReceiptRepository.GetByIdAsync(receiptId);
           receipt.IsCheckedOut = true;
           _unitOfWork.ReceiptRepository.Update(receipt);
           await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var temp = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(modelId);
            await _unitOfWork.ReceiptRepository.DeleteByIdAsync(modelId);

            var recDetals = temp.ReceiptDetails.Where(t => t.ReceiptId == modelId);

            foreach (var item in recDetals)
            {
                _unitOfWork.ReceiptDetailRepository.Delete(item);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            List<ReceiptModel> rml = new List<ReceiptModel>();
            foreach (Receipt r in receipts) {
               rml.Add(_mapper.Map<Receipt,ReceiptModel>(r));
            }
            return rml.AsEnumerable<ReceiptModel>();
        }


        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(id);
            var value = _mapper.Map<Receipt, ReceiptModel>(receipt);
            return value;
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            List<ReceiptDetailModel> rdml = new List<ReceiptDetailModel>();
            foreach (ReceiptDetail rd in receipt.ReceiptDetails) {
                rdml.Add(_mapper.Map<ReceiptDetail, ReceiptDetailModel>(rd));
            };
            return rdml;
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            receipts = receipts.Where(r=>r.OperationDate >= startDate && r.OperationDate<=endDate).ToList();
            List<ReceiptModel> rml = new List<ReceiptModel>();
            foreach (Receipt r in receipts) {
                rml.Add(_mapper.Map<Receipt, ReceiptModel>(r));
            }
            return rml;
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId);
            var receiptDetail = receipt.ReceiptDetails.Where(rd => rd.ProductId == productId).ToArray();
            var a = receiptDetail.Select(rd => rd.Quantity - quantity).ToArray();
            int b;
            for (int i = 0; i < receiptDetail.Count(); i++) {
                receiptDetail[i].Quantity = receiptDetail[i].Quantity - quantity;
                b = receiptDetail[i].Quantity; 
                if (receiptDetail[i].Quantity <= 0) {
                    _unitOfWork.ReceiptDetailRepository.Delete(receiptDetail[i]);
                }
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            decimal toPay = 0;
            var receipt = await _unitOfWork.ReceiptRepository.GetByIdWithDetailsAsync(receiptId)
                ?? throw new MarketException("receipt is null");
            
            foreach (ReceiptDetail rd in receipt.ReceiptDetails) {
                toPay += rd.DiscountUnitPrice * rd.Quantity;
            }
            return toPay;   
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            var receipt = _mapper.Map<ReceiptModel, Receipt>(model);
            _unitOfWork.ReceiptRepository.Update(receipt);
            await _unitOfWork.SaveAsync();
        }
    }
}
