using System;
using System.Threading.Tasks;
using StormShop.Core.Db;

namespace StormShop.Infrastructure.Mongo
{
    public abstract class BaseMongoUnitOfWork : IUnitOfWork
    {
        protected BaseMongoUnitOfWork(IMongoContext context)
        {
            _dbContext = context;
        }

        private readonly IMongoContext _dbContext;
        private bool _isDisposed;

        public async Task<bool> Commit()
        {
            var changeAmount = await _dbContext.SaveChanges();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                _dbContext.Dispose();
            }

            _isDisposed = true;
        }
        
        ~BaseMongoUnitOfWork()
        {
            Dispose(false);
        }
    }
}
