using DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Picu.Services.Interfaces
{
    public interface IPicuService
    {
        #region Get

        IQueryable<T> Set<T>() where T : EntityBase;

        T Get<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes) where T : EntityBase;

        T Find<T>(int id) where T : EntityBase;

        #endregion

        #region Add

        void Add<T>(T entity) where T : EntityBase;

        void AddAll<T>(IEnumerable<T> entities) where T : EntityBase;

        #endregion

        #region Update

        void Update<T>(T entity) where T : EntityBase;

        void UpdateAll<T>(IEnumerable<T> entities) where T : EntityBase;

        #endregion

        #region Remove

        void Remove<T>(T entity) where T : EntityBase;

        void RemoveAll<T>(IEnumerable<T> entities) where T : EntityBase;

        #endregion

        #region Utils

        void SaveChanges();

        #endregion
    }
}
