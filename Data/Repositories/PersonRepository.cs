using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Data.Data;
using System.Linq;

namespace Data.Repositories
{
    public class PersonRepository :Repository<Person>, IPersonRepository
    {
        private readonly TradeMarketDbContext context;
        public PersonRepository(TradeMarketDbContext context):base(context) {
            this.context = context;
        }
    }
}
