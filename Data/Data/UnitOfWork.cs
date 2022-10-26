using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TradeMarketDbContext _context;
        public UnitOfWork(TradeMarketDbContext context) {
            _context = context;
        }
        public ICustomerRepository CustomerRepository => new CustomerRepository(_context);

        public IPersonRepository PersonRepository => new PersonRepository(_context);

        public IProductRepository ProductRepository => new ProductRepository(_context);

        public IProductCategoryRepository ProductCategoryRepository => new ProductCategoryRepository(_context);

        public IReceiptRepository ReceiptRepository => new ReceiptRepository(_context);

        public IReceiptDetailRepository ReceiptDetailRepository => new ReceiptDetailRepository(_context);

        public async Task SaveAsync()
        {
          await _context.SaveChangesAsync();
        }

    }
}