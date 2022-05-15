using DataAccess.Base.Interfaces;
using System;
using System.Collections.Generic;

namespace DataAccess.Base
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly Dictionary<string, object> _repositories;
        private readonly BaseContext _context;
        private bool _disposed;

        public UnitOfWork(BaseContext context)
        {
            _context = context;
            _repositories = new Dictionary<string, object>();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public BaseContext GetContext()
        {
            return _context;
        }

        public IRepository<T> Repository<T>() where T : EntityBase
        {
            var type = typeof(T).Name;

            if (_repositories.ContainsKey(type))
                return (Repository<T>)_repositories[type];

            var repositoryType = typeof(Repository<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
            _repositories.Add(type, repositoryInstance);

            return (Repository<T>)_repositories[type];
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}

