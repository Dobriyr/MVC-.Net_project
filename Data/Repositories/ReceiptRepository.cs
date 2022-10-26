﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Data.Entities;
using Data.Interfaces;
using Data.Data;


namespace Data.Repositories
{

    public class ReceiptRepository :Repository<Receipt>, IReceiptRepository
    {

        private readonly TradeMarketDbContext context;
        public ReceiptRepository(TradeMarketDbContext context):base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await this.context.Receipts.
                Include(r => r.Customer).
                ThenInclude(r => r.Person).
                Include(r => r.ReceiptDetails).
                ThenInclude(rd => rd.Product).
                ThenInclude(p => p.Category).
                ToListAsync();
        }


        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await context.Receipts.
                Include(r => r.Customer).
                Include(r => r.ReceiptDetails).
                ThenInclude(rd => rd.Product).
                ThenInclude(p => p.Category).
                FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
