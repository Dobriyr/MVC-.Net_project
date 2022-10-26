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

    public class ProductRepository : Repository<Product>, IProductRepository
    {

        private readonly TradeMarketDbContext context;
        public ProductRepository(TradeMarketDbContext context): base(context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await this.context.Products.
                Include(pc => pc.Category).
                Include(rd => rd.ReceiptDetails).
                ThenInclude(r => r.Receipt).
                ToListAsync();
                
        }
        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return await this.context.Products.
                Include(pc => pc.Category).
                Include(rd => rd.ReceiptDetails).
                ThenInclude(r => r.Receipt).
                FirstOrDefaultAsync(x=>x.Id==id);
        }
    }
}
