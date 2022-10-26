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
    public class ReceiptDetailRepository : Repository<ReceiptDetail>, IReceiptDetailRepository
    {

        private readonly TradeMarketDbContext context;
        public ReceiptDetailRepository(TradeMarketDbContext context): base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await context.ReceiptsDetails.
                Include(rd => rd.Receipt).
                Include(rd=>rd.Product).
                ThenInclude(p=>p.Category).
                ToListAsync();
        }
    }
}
