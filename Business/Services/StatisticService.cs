using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System.Linq;
using Data.Entities;
using Business.Validation;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public StatisticService(IUnitOfWork uof, IMapper mapper)
        {
            this._unitOfWork = uof;
            this._mapper = mapper;
        }


        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {

            var receipt = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            
            var products = receipt
                .Where(r => r.Customer.Id == customerId)
                .SelectMany(r => r.ReceiptDetails)
                .GroupBy(r => r.Product)
                .Select(r =>
                    new ReceiptDetail
                    {
                        Id = 0,
                        Product = r.Key,
                        ProductId = r.Key.Id,
                        ReceiptId = 0,
                        DiscountUnitPrice = 0,
                        UnitPrice = 0,
                        Quantity = r.Sum(r => r.Quantity),
                    }
                )
                .OrderBy(r => r.Quantity).Select(rdm => rdm.Product).Take(productCount).ToList();

            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(products);

        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipt  = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();

            receipt = receipt.Where(r => r.OperationDate > startDate && r.OperationDate < endDate).ToList();
            var sum = receipt.Sum(r => r.ReceiptDetails.Where(rd => rd.Product.Category.Id == categoryId)
            .Sum(r => r.Quantity * r.DiscountUnitPrice));
            return sum;


           // var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
           // var sum =
           // receipts.Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
           //     .SelectMany(r => r.ReceiptDetails).Where(rd => rd.Product.ProductCategoryId == categoryId)
           //     .Select(rd => rd.DiscountUnitPrice * rd.Quantity).OrderBy(x => x).ToList();
           // var suma = sum.First();
           // return suma;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetails = await _unitOfWork.ReceiptDetailRepository.GetAllWithDetailsAsync();
            var product = receiptDetails.OrderByDescending(rd => rd.Quantity).Select(rd => rd.Product).Take(productCount);
            return _mapper.Map<IEnumerable<Product> ,IEnumerable<ProductModel>>(product);
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var Cusomers = receipts.GroupBy(r => r.Customer).
                Select(a =>
                    new CustomerActivityModel
                    {
                        CustomerId = a.Key.Id,
                        CustomerName = (a.Key.Person.Name + " " + a.Key.Person.Surname),
                        ReceiptSum = receipts.Where(r => r.CustomerId == a.Key.Id).Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate)
                        .SelectMany(r => r.ReceiptDetails).Sum(rd => rd.Quantity * rd.DiscountUnitPrice)

                    }
                ).OrderByDescending(acm => acm.ReceiptSum).Take(customerCount).ToList();
            return Cusomers;
        }
    }
}
