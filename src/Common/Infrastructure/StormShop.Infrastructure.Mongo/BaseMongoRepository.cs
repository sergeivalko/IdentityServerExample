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
        protected readonly IMongoContext Context;
        protected readonly IMongoCollection<TEntity> DbSet;

        protected BaseMongoRepository(IMongoContext context)
        {
            Context = context;
            DbSet = context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.Find(Builders<TEntity>.Filter.Empty).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.Find(Builders<TEntity>.Filter.Empty).ToListAsync();
        }

        public virtual IEnumerable<TEntity> FilterBy(
            Expression<Func<TEntity, bool>> filterExpression)
        {
            return DbSet.Find(filterExpression).ToEnumerable();
        }

        public virtual async Task<IEnumerable<TEntity>> FilterByAsync(
            Expression<Func<TEntity, bool>> filterExpression)
        {
            return await DbSet.Find(filterExpression).ToListAsync();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return DbSet.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual async Task<IEnumerable<TProjected>> FilterByAsync<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return await DbSet.Find(filterExpression).Project(projectionExpression).ToListAsync();
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            return DbSet.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => DbSet.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TEntity FindById(object id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            return DbSet.Find(filter).SingleOrDefault();
        }

        public virtual Task<TEntity> FindByIdAsync(object id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TEntity>.Filter.Eq("_id", id);
                return DbSet.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual void Add(TEntity entity)
        {
            Context.AddCommand(() => DbSet.InsertOneAsync(entity));
        }

        public void AddRange(ICollection<TEntity> entities)
        {
            Context.AddCommand(() => DbSet.InsertManyAsync(entities));
        }

        public void Update(TEntity entity)
        {
            var id = entity.GetType().GetProperty("Id")?.GetValue(entity, null);
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            Context.AddCommand(() => DbSet.FindOneAndReplaceAsync(filter, entity));
        }

        public void Remove(Expression<Func<TEntity, bool>> filterExpression)
        {
            Context.AddCommand(() => DbSet.FindOneAndDeleteAsync(filterExpression));
        }

        public void RemoveById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            Context.AddCommand(() => DbSet.FindOneAndDeleteAsync(filter));
        }

        public void RemoveRange(Expression<Func<TEntity, bool>> filterExpression)
        {
            Context.AddCommand(() => DbSet.DeleteManyAsync(filterExpression));
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
