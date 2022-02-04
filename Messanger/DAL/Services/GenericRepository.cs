using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Models;
using DAL.Abstractions.Interfaces;
using DAL.DataBase;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : IdKey
    {
        private readonly DALContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DALContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        // public async Task<IEnumerable<TEntity>> GetAll()
        // {
        //     return await _context.
        // }
        

        public async Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        
        public async Task Insert(TEntity obj)
        {
            await _dbSet.AddAsync(obj);
        }

        public async Task InsertRange(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Delete(TEntity obj)
        {
            if (_context.Entry(obj).State == EntityState.Detached)
            {
                _dbSet.Attach(obj);
            }
            
            _dbSet.Remove(obj);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
        
        public async Task DeleteById(int id)
        {
            var obj = await _dbSet.FindAsync(id);
            
            this.Delete(obj);
        }

        public void Update(TEntity obj)
        {
            _dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}