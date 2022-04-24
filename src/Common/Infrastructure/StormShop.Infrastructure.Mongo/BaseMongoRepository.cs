using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using StormShop.Core.Db;

namespace StormShop.Infrastructure.Mongo
{
    public abstract class BaseMongoRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoContext _context;
        private readonly IMongoCollection<TEntity> _dbSet;
        private bool _isDisposed;

        protected BaseMongoRepository(IMongoContext context)
        {
            _context = context;
            _dbSet = context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbSet.Find(Builders<TEntity>.Filter.Empty).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
        }

        public virtual IEnumerable<TEntity> FilterBy(
            Expression<Func<TEntity, bool>> filterExpression)
        {
            return _dbSet.Find(filterExpression).ToEnumerable();
        }

        public virtual async Task<IEnumerable<TEntity>> FilterByAsync(
            Expression<Func<TEntity, bool>> filterExpression)
        {
            return await _dbSet.Find(filterExpression).ToListAsync();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return _dbSet.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual async Task<IEnumerable<TProjected>> FilterByAsync<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return await _dbSet.Find(filterExpression).Project(projectionExpression).ToListAsync();
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            return _dbSet.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => _dbSet.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TEntity FindById(object id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            return _dbSet.Find(filter).SingleOrDefault();
        }

        public virtual Task<TEntity> FindByIdAsync(object id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TEntity>.Filter.Eq("_id", id);
                return _dbSet.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual void Add(TEntity entity)
        {
            _context.AddCommand(() => _dbSet.InsertOneAsync(entity));
        }

        public void AddRange(ICollection<TEntity> entities)
        {
            _context.AddCommand(() => _dbSet.InsertManyAsync(entities));
        }

        public void Update(TEntity entity)
        {
            var id = entity.GetType().GetProperty("Id")?.GetValue(entity, null);
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            _context.AddCommand(() => _dbSet.FindOneAndReplaceAsync(filter, entity));
        }

        public void Remove(Expression<Func<TEntity, bool>> filterExpression)
        {
            _context.AddCommand(() => _dbSet.FindOneAndDeleteAsync(filterExpression));
        }

        public void RemoveById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            _context.AddCommand(() => _dbSet.FindOneAndDeleteAsync(filter));
        }

        public void RemoveRange(Expression<Func<TEntity, bool>> filterExpression)
        {
            _context.AddCommand(() => _dbSet.DeleteManyAsync(filterExpression));
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
                _context?.Dispose();
            }

            _isDisposed = true;
        }
        
        ~BaseMongoRepository()
        {
            Dispose(false);
        }
    }
}
