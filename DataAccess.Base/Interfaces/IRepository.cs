using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Base.Interfaces
{
    public interface IRepository<T> where T : EntityBase
    {
        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        void Remove(Expression<Func<T, bool>> where);

        T Find(int id);

        T Get(Expression<Func<T, bool>> where);

        T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);

        IEnumerable<T> GetAll();

        IQueryable<T> Set();

        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);

        long Count();

        long Count(Expression<Func<T, bool>> where);

        void SaveChanges();
    }
}