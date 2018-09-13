////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Repositories\Repository.cs
//
// summary:	Implements the repository class
////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using LinqKit;
using InventoryApp_DL.DataContext;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Infrastructure;
using InventoryApp_DL.Interfaces;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;


namespace InventoryApp_DL.Repositories
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A repository. </summary>
    ///
    /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
    ///
    /// <typeparam name="TEntity">  Type of the entity. </typeparam>
    ///
    /// <seealso cref="T:InventoryApp_DL.Repositories.IRepository{TEntity}"/>
    /// <seealso cref="T:InventoryApp_DL.Repositories.IRepositoryAsync{TEntity}"/>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Repository<TEntity> :
        IRepository<TEntity>,
        IRepositoryAsync<TEntity> where TEntity : Entity
    {
        #region Enumerations

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Defines the change actions for a data operation. </summary>
        ///
        /// <remarks>   (ST), TechArk Solutions, 03/19/2014. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public enum ChangeAction
        {
            /// <summary>   An enum constant representing the undefined option. </summary>
            Undefined,
            /// <summary>   An enum constant representing the created option. </summary>
            Created,
            /// <summary>   An enum constant representing the updated option. </summary>
            Updated,
            /// <summary>   An enum constant representing the deleted option. </summary>
            Deleted,
        };

        #endregion

        #region Private Readonly Member Variables

        /// <summary>   Context for the data. </summary>
        private readonly IDataContextAsync dataContext;



        /// <summary>   Set the entity database belongs to. </summary>
        private readonly DbSet<TEntity> entityDbSet;

        #endregion

        #region Public Properties

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the set the entity database belongs to. </summary>
        ///
        /// <value> The entity database set. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public DbSet<TEntity> EntityDbSet
        {
            get { return entityDbSet; }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets a context for the database. </summary>
        ///
        /// <value> The database context. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public System.Data.Entity.DbContext DbContext
        {
            get
            {
                System.Data.Entity.DbContext db = (dataContext as System.Data.Entity.DbContext);
                db.Database.CommandTimeout = 180;
                return db;
            }
        }

        #endregion

        #region Constructors

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="context">  The context. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public Repository(IDataContextAsync context)
        {
            dataContext = context;
            entityDbSet = context.Set<TEntity>();
        }

        #region Static Constructor

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Static constructor. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        static Repository()
        {
            // auditLogDataTableMaster = CreateAuditLogDataTable();

            // Create a mapping for the TEntity ignoring the ID value.
            // AutoMapper.Mapper.CreateMap<TEntity, TEntity>().ForMember(x => ((IEntityActive)(x)).ID, exp => exp.Ignore());
        }

        #endregion

        #endregion

        #region Public Methods

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Enumerates execute with store procedure in this collection. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="query">        The query. </param>
        /// <param name="parameters">   Options for controlling the operation. </param>
        ///
        /// <returns>
        ///     An enumerator that allows foreach to be used to process execute with store procedure in
        ///     this collection.
        /// </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IEnumerable<TEntity> ExecWithStoreProcedure(string query, params Object[] parameters)
        {
            return DbContext.Database.SqlQuery<TEntity>(query, parameters);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Searches for the first match for the given parameters object[]. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="keyValues">    A variable-length parameters list containing key values. </param>
        ///
        /// <returns>   A TEntity. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual TEntity Find(params Object[] keyValues)
        {
            return entityDbSet.Find(keyValues);
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return entityDbSet.FirstOrDefault(predicate);
        }

        public virtual IQueryable<TEntity> All()
        {
            return entityDbSet.AsQueryable();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Select query. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="query">        The query. </param>
        /// <param name="parameters">   Options for controlling the operation. </param>
        ///
        /// <returns>   An IQueryable&lt;TEntity&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual IQueryable<TEntity> SelectQuery(string query, params Object[] parameters)
        {
            return entityDbSet.SqlQuery(query, parameters).AsQueryable();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Inserts the given entity. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="entity">   The entity. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Insert(TEntity entity)
        {
            ((IObjectState)entity).ObjectState = ObjectState.Added;
            entityDbSet.Attach(entity);
            dataContext.SyncObjectState(entity);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Inserts a range described by entities. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="entities"> The entities. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Inserts a graph described by entity. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="entity">   The entity. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void InsertGraph(TEntity entity)
        {
            entityDbSet.Add(entity);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Inserts a graph range described by entities. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="entities"> The entities. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            entityDbSet.AddRange(entities);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the given entity. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="entity">   The entity. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Update(TEntity entity)
        {
            entityDbSet.Attach(entity);
            ((IObjectState)entity).ObjectState = ObjectState.Modified;
            dataContext.SyncObjectState(entity);

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the given ID. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="id">   The identifier to delete. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Delete(Object id)
        {
            var entity = entityDbSet.Find(id);
            Delete(entity);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the given entity. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="entity">   The entity to delete. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual void Delete(TEntity entity)
        {
            ((IObjectState)entity).ObjectState = ObjectState.Deleted;
            entityDbSet.Attach(entity);
            dataContext.SyncObjectState(entity);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the query. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <returns>   An IQueryFluent&lt;TEntity&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public IQueryFluent<TEntity> Query()
        {
            return new QueryFluent<TEntity>(this);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Queries the given query object. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="queryObject">  The query object. </param>
        ///
        /// <returns>   An IQueryFluent&lt;TEntity&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual IQueryFluent<TEntity> Query(QueryObject<TEntity> queryObject)
        {
            return new QueryFluent<TEntity>(this, queryObject);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Queries the given query. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="query">    The query. </param>
        ///
        /// <returns>   An IQueryFluent&lt;TEntity&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, Boolean>> query)
        {
            return new QueryFluent<TEntity>(this, query);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Searches for the first asynchronous. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="keyValues">    A variable-length parameters list containing key values. </param>
        ///
        /// <returns>   The found asynchronous. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual async Task<TEntity> FindAsync(params Object[] keyValues)
        {
            return await entityDbSet.FindAsync(keyValues);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Searches for the first asynchronous. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="cancellationToken">    The cancellation token. </param>
        /// <param name="keyValues">
        ///     A variable-length parameters list containing key values.
        /// </param>
        ///
        /// <returns>   The found asynchronous. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params Object[] keyValues)
        {
            return await entityDbSet.FindAsync(cancellationToken, keyValues);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the asynchronous described by keyValues. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="keyValues">    A variable-length parameters list containing key values. </param>
        ///
        /// <returns>   A Task&lt;Boolean&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual async Task<Boolean> DeleteAsync(params Object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the asynchronous. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="cancellationToken">    The cancellation token. </param>
        /// <param name="keyValues">
        ///     A variable-length parameters list containing key values.
        /// </param>
        ///
        /// <returns>   A Task&lt;Boolean&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public virtual async Task<Boolean> DeleteAsync(CancellationToken cancellationToken, params Object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);
            if (entity == null) return false;

            entityDbSet.Attach(entity);
            entityDbSet.Remove(entity);
            return true;
        }

        //public IQueryable ODataQueryable(ODataQueryOptions<TEntity> oDataQueryOptions)
        //{
        //    return oDataQueryOptions.ApplyTo(entityDbSet);
        //}

        #endregion

        #region Internal Methods

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Selects. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="filter">   (Optional) specifies the filter. </param>
        /// <param name="orderBy">  (Optional) amount to order by. </param>
        /// <param name="includes"> (Optional) the includes. </param>
        /// <param name="page">     (Optional) the page. </param>
        /// <param name="pageSize"> (Optional) size of the page. </param>
        ///
        /// <returns>   An IQueryable&lt;TEntity&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        internal (IQueryable<TEntity>, int) Select(
            Expression<Func<TEntity, Boolean>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, Object>>> includes = null,
            Int32? page = null,
            Int32? pageSize = null)
        {
            IQueryable<TEntity> query = entityDbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);

            if (filter != null)
                query = query.AsExpandable().Where(filter);

            int TotalRecords = query.Count();

            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return (query, TotalRecords);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Select asynchronous. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="query">    (Optional) The query. </param>
        /// <param name="orderBy">  (Optional) amount to order by. </param>
        /// <param name="includes"> (Optional) the includes. </param>
        /// <param name="page">     (Optional) the page. </param>
        /// <param name="pageSize"> (Optional) size of the page. </param>
        ///
        /// <returns>   A Task&lt;IEnumerable&lt;TEntity&gt;&gt; </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        internal async Task<(IEnumerable<TEntity>, int)> SelectAsync(
            Expression<Func<TEntity, Boolean>> query = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, Object>>> includes = null,
            Int32? page = null,
            Int32? pageSize = null)
        {
            (IQueryable<TEntity>, int) result = Select(query, orderBy, includes, page, pageSize);
            return (result.Item1.AsEnumerable(), result.Item2);
        }

        #endregion

        #region Private Static Methods
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Perfom audited data operation. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="entity">       The entity. </param>
        /// <param name="auditUserID">  Identifier for the audit user. </param>
        /// <param name="changeAction"> The change action. </param>
        ///
        /// <returns>   A Task. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static async Task PerfomAuditedDataOperation(TEntity entity, ChangeAction changeAction)
        {
            try
            {
                using (IDataContextAsync dataContext = new DataContext.DataContext())
                {
                    Repository<TEntity> repository = new Repository<TEntity>(dataContext);

                    using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(dataContext))
                    {
                        //unitOfWork.BeginTransaction();

                        try
                        {
                            if ((changeAction == ChangeAction.Deleted))
                            {
                                repository.Delete(entity);
                                await unitOfWork.SaveChangesAsync();
                            }

                            if ((changeAction == ChangeAction.Updated))
                            {
                                // Update the database entity.
                                repository.Update(entity);

                                // Save changes.
                                await unitOfWork.SaveChangesAsync();
                            }
                            else if (changeAction == ChangeAction.Created)
                            {
                                repository.Insert(entity);
                                // Save the entity.  This will trigger the setting of the ID property value.
                                var changes = await unitOfWork.SaveChangesAsync();
                            }
                            // Commit the transaction.
                            //unitOfWork.Commit();
                        }
                        catch (Exception ex)
                        {

                            // Was this a DbEntityValidationException exception?
                            DbEntityValidationException entityValidationException = (ex as DbEntityValidationException);

                            if (entityValidationException != null)
                            {
                                foreach (DbEntityValidationResult validationErrors in entityValidationException.EntityValidationErrors)
                                {
                                    foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                                    {
                                        System.Diagnostics.Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                                    }
                                }
                            }

                            // Rollback transaction
                            unitOfWork.Rollback();

                            // Rethrow transaction.
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Public Static Methods

        public static async void DeleteRange(IEnumerable<TEntity> entities)
        {
            using (IDataContextAsync dataContext = new DataContext.DataContext())
            {
                Repository<TEntity> repository = new Repository<TEntity>(dataContext);

                using (IUnitOfWorkAsync unitOfWork = new UnitOfWork(dataContext))
                {
                    //unitOfWork.BeginTransaction();

                    try
                    {
                        foreach (var entity in entities)
                        {
                            repository.Delete(entity);
                        }
                        await unitOfWork.SaveChangesAsync();

                    }
                    catch (Exception ex) { }
                }
            }



        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets entity by identifier. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="entityID"> Identifier for the entity. </param>
        ///
        /// <returns>   The entity by identifier. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task<TEntity> GetEntityByID(Int32 entityID)
        {
            TEntity entity = null;

            using (IDataContextAsync dataContext = new DataContext.DataContext())
            {
                Repository<TEntity> repository = new Repository<TEntity>(dataContext);

                entity = await repository.FindAsync(entityID);
            }

            return (entity);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets entity list for query. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <param name="query">    The query. </param>
        /// <param name="orderBy">  (Optional) amount to order by. </param>
        /// <param name="includes"> (Optional) the includes. </param>
        /// <param name="page">     (Optional) the page. </param>
        /// <param name="pageSize"> (Optional) size of the page. </param>
        ///
        /// <returns>   The entity list for query. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static (List<TEntity>,int) GetEntityListForQuery
            (Expression<Func<TEntity, Boolean>> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, Object>>> includes = null,
            Int32? page = null,
            Int32? pageSize = null)
        {
            List<TEntity> entities = null;
            int TotalRecords = 0;
            using (IDataContextAsync dataContext = new DataContext.DataContext())
            {
                Repository<TEntity> repository = new Repository<TEntity>(dataContext);

                (IEnumerable<TEntity>, int) result = repository.SelectAsync(query, orderBy, includes, page, pageSize).Result;

                entities = result.Item1.ToList();
                TotalRecords = result.Item2;
            }

            return (entities,TotalRecords);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Inserts an entity. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <typeparam name="U">    Generic type parameter. </typeparam>
        /// <param name="entity">               The entity. </param>
        /// <param name="UserId">               Identifier for the user. </param>
        /// <param name="keyValueGenerator">    The key value generator. </param>
        ///
        /// <returns>   A Task. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task InsertEntity<U>(TEntity entity, Func<TEntity, U> keyValueGenerator)
        {
            Int32 ID = Convert.ToInt32(keyValueGenerator(entity));
            await PerfomAuditedDataOperation(entity, ChangeAction.Created);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the entity. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <typeparam name="U">    Generic type parameter. </typeparam>
        /// <param name="entity">               The entity. </param>
        /// <param name="UserId">               Identifier for the user. </param>
        /// <param name="keyValueGenerator">    The key value generator. </param>
        ///
        /// <returns>   A Task. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task UpdateEntity<U>(TEntity entity, Func<TEntity, U> keyValueGenerator)
        {
            Int32 ID = Convert.ToInt32(keyValueGenerator(entity));
            await PerfomAuditedDataOperation(entity, ChangeAction.Updated);
        }
        /// <summary>
        /// This method is creted to update the entity which is having id as string or guid.
        /// </summary>
        /// <typeparam name="U">U</typeparam>
        /// <param name="entity">entity</param>
        /// <param name="keyValueGenerator">keyValueGenerator</param>
        /// <returns>Task</returns>
        public static async Task UpdateUserEntity<U>(TEntity entity, Func<TEntity, U> keyValueGenerator)
        {
            string ID = keyValueGenerator(entity).ToString();
            await PerfomAuditedDataOperation(entity, ChangeAction.Updated);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Deletes the entity. </summary>
        ///
        /// <remarks>   (MM), TechArk Solutions, 03/19/2014. </remarks>
        ///
        /// <typeparam name="U">    Generic type parameter. </typeparam>
        /// <param name="entity">               The entity. </param>
        /// <param name="UserId">               Identifier for the user. </param>
        /// <param name="keyValueGenerator">    The key value generator. </param>
        ///
        /// <returns>   A Task. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static async Task DeleteEntity<U>(TEntity entity, Func<TEntity, U> keyValueGenerator)
        {
            await PerfomAuditedDataOperation(entity, ChangeAction.Deleted);
        }
        #endregion
    }

    public class PagedResult<TEntity>
    {
        public int TotalRecords { get; set; }
        public IEnumerable<TEntity> entities { get; set; }

        public PagedResult(int TotalRecords, IEnumerable<TEntity> entities)
        {
            this.TotalRecords = TotalRecords;
            this.entities = entities;
        }
    }

}
