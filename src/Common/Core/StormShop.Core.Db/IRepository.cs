using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StormShop.Core.Db
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();

        IEnumerable<TEntity> FilterBy(
            Expression<Func<TEntity, bool>> filterExpression);

        Task<IEnumerable<TEntity>> FilterByAsync(
            Expression<Func<TEntity, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression);

        Task<IEnumerable<TProjected>> FilterByAsync<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression);

        TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression);
        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression);
        TEntity FindById(object id);
        Task<TEntity> FindByIdAsync(object id);
        void Add(TEntity entity);
        void AddRange(ICollection<TEntity> entities);
        void Update(TEntity entity);
        void Remove(Expression<Func<TEntity, bool>> filterExpression);
        void RemoveById(string id);
        void RemoveRange(Expression<Func<TEntity, bool>> filterExpression);
    }
}
