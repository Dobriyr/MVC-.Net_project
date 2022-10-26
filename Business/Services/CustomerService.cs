using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Business.Validation;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public CustomerService(IUnitOfWork uof, IMapper mapper) {
            this._unitOfWork = uof;
            this._mapper = mapper;
        }

        public async Task AddAsync(CustomerModel model)
        {
            model.IsValid();
            var customer = _mapper.Map<CustomerModel, Customer>(model);
            
            await _unitOfWork.CustomerRepository.AddAsync(customer);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _unitOfWork.CustomerRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            var customersModels = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerModel>>(customers);
            return customersModels;
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            var customerModels = _mapper.Map<Customer, CustomerModel>(customer);
            return customerModels;
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<CustomerModel>>
                (customers.Where(c => c.Receipts
                         .SelectMany(r => r.ReceiptDetails)
                         .Any(rd => rd.ProductId == productId)));
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            model.IsValid();
            var customer = _mapper.Map<CustomerModel, Customer>(model);
            
            _unitOfWork.CustomerRepository.Update(customer);
            _unitOfWork.PersonRepository.Update(customer.Person);
            await _unitOfWork.SaveAsync();
        }
    }
}
