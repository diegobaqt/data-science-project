using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.Base.Interfaces;

namespace DataAccess.Base
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        #region Private Fields

        private readonly DbSet<T> _entitySet;
        private readonly DbContext _context;

        #endregion Private Fields

        #region Constructors

        public Repository(DbContext context)
        {
            _context = context;
            _entitySet = context.Set<T>();
        }

        #endregion

        #region Public Persistence Methods

        public void Add(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void Remove(Expression<Func<T, bool>> where)
        {
            var entities = _entitySet.Where(where).AsEnumerable();
            foreach (var entity in entities)
                _entitySet.Remove(entity);
        }

        #endregion

        #region Public Read Methods

        public T Find(int id)
        {
            return _entitySet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _entitySet.AsNoTracking().Where(where).FirstOrDefault();
        }

        public T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = _entitySet.AsNoTracking().Where(where);
            var result = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return result.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return _entitySet.AsNoTracking();
        }

        public IQueryable<T> Set()
        {
            return _entitySet.AsNoTracking().AsQueryable();
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _entitySet.AsNoTracking();

            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public virtual long Count()
        {
            return _entitySet.AsNoTracking().Count();
        }

        public long Count(Expression<Func<T, bool>> where)
        {
            return _entitySet.AsNoTracking().Where(where).Count();
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        #endregion
    }
}

