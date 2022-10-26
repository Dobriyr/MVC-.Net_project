using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Entities;
using Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity : BaseEntity
    {

        private readonly TradeMarketDbContext context;
        public Repository(TradeMarketDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await this.context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            context.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var ToDelete = await context.FindAsync<TEntity>(id);
            this.context.Remove<TEntity>(ToDelete);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await context.FindAsync<TEntity>(id);
        }

        public void Update(TEntity entity)
        {
            this.context.Update(entity);
         }
    }
}
