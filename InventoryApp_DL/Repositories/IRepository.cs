#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using InventoryApp_DL.Infrastructure;

#endregion

namespace InventoryApp_DL.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void InsertGraph(TEntity entity);
        void InsertGraphRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        IQueryFluent<TEntity> Query(QueryObject<TEntity> queryObject);

        IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query);

        IQueryFluent<TEntity> Query();

        

        //IQueryable ODataQueryable(ODataQueryOptions<TEntity> oDataQueryOptions);
    }
}