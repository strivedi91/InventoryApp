using System;
using System.Linq.Expressions;

namespace InventoryApp_DL.Repositories
{
    public interface IQueryObject<TEntity>
    {
        Expression<Func<TEntity, bool>> Query();
        Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query);
        Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query);
        Expression<Func<TEntity, bool>> And(QueryObject<TEntity> queryObject);
        Expression<Func<TEntity, bool>> Or(QueryObject<TEntity> queryObject);
    }
}