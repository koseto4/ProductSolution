using Microsoft.EntityFrameworkCore;
using ProductsSystem.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsSystem.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        protected DbSet<T> _dbSet { get; set; }

        protected ProductsSystemDbContext _context { get; set; }

        public GenericRepository(ProductsSystemDbContext context)
        {
            _context = context;
            this._dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void DeleteById(int id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var existing = await _dbSet.FindAsync(id);
            _context.Entry(existing).CurrentValues.SetValues(entity);
        }

    }
}
