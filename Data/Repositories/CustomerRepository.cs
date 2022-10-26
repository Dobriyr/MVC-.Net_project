using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Data.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories
{
    public class CustomerRepository: Repository<Customer>, ICustomerRepository
    {
        private readonly TradeMarketDbContext context;
        public CustomerRepository(TradeMarketDbContext context): base(context)
        {
            this.context = context;
        }

        

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return await this.context.Customers.
                         Include(c => c.Person).
                         Include(c => c.Receipts).
                         ThenInclude(r => r.ReceiptDetails)
                         .ToListAsync();
        }
        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return await this.context.Customers.
                        Include(c => c.Person).
                        Include(c=>c.Receipts).
                        ThenInclude(r => r.ReceiptDetails).
                        ThenInclude(rd=>rd.Product)
                        .FirstOrDefaultAsync(c=>c.Id==id);
        }
    }
}
