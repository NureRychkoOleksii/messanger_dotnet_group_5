using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Models;

namespace DAL.Abstractions.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : IdKey
    {
        //public Task<IEnumerable<TEntity>> GetAll();

        //public Task<IEnumerable<TEntity>> GetAllByCondition(Func<TEntity, bool> condition);

        public Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        public Task Insert(TEntity obj);

        public void Delete(TEntity obj);

        public Task DeleteById(int id);

        public void Update(TEntity obj);

        public Task InsertRange(IEnumerable<TEntity> entities);

        public void DeleteRange(IEnumerable<TEntity> entities);
    }
}