using DataAccess.Base;
using DataAccess.Base.Interfaces;
using Picu.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Picu.Services
{
    public class PicuService : IPicuService
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Constructors

        public PicuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Get

        public T Get<T>(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes) where T : EntityBase
        {
            return _unitOfWork.Repository<T>().Get(where, includes);
        }

        public T Find<T>(int id) where T : EntityBase
        {
            return _unitOfWork.Repository<T>().Find(id);
        }

        public IQueryable<T> Set<T>() where T : EntityBase
        {
            return _unitOfWork.Repository<T>().Set();
        }

        #endregion

        #region Add

        public void Add<T>(T entity) where T : EntityBase
        {
            _unitOfWork.Repository<T>().Add(entity);
            _unitOfWork.SaveChanges();
        }

        public void AddAll<T>(IEnumerable<T> entities) where T : EntityBase
        {
            foreach (var entity in entities)
                _unitOfWork.Repository<T>().Add(entity);

            _unitOfWork.SaveChanges();
        }

        #endregion

        #region Update

        public void Update<T>(T entity) where T : EntityBase
        {
            _unitOfWork.Repository<T>().Update(entity);
            _unitOfWork.SaveChanges();
        }

        public void UpdateAll<T>(IEnumerable<T> entities) where T : EntityBase
        {
            foreach (var entity in entities)
                _unitOfWork.Repository<T>().Update(entity);

            _unitOfWork.SaveChanges();
        }

        #endregion

        #region Remove

        public void Remove<T>(T entity) where T : EntityBase
        {
            _unitOfWork.Repository<T>().Remove(entity);
            _unitOfWork.SaveChanges();
        }

        public void RemoveAll<T>(IEnumerable<T> entities) where T : EntityBase
        {
            foreach (var entity in entities)
                _unitOfWork.Repository<T>().Remove(entity);

            _unitOfWork.SaveChanges();
        }

        #endregion

        #region Utils

        public void SaveChanges()
        {
            _unitOfWork.SaveChanges();
        }

        #endregion


    }
}
