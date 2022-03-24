using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StormShop.Core.Db;

namespace StormShop.Infrastructure.Mongo
{
    public abstract class BaseMongoUnitOfWork : IUnitOfWork
    {
        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public BaseMongoUnitOfWork(IMongoContext context)
        {
            DbContext = context;
        }

        public IMongoContext DbContext { get; }

        public async Task<bool> Commit()
        {
            var changeAmount = await DbContext.SaveChanges();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
