using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using InventoryApp_DL.DataContext;
using InventoryApp_DL.Infrastructure;
using InventoryApp_DL.Repositories;
using InventoryApp_DL.Interfaces;

namespace InventoryApp_DL
{
    public class UnitOfWork : IUnitOfWork, IUnitOfWorkAsync
    {
        #region Private Fields

        private readonly IDataContextAsync _dataContext;
        private bool _disposed;
        private ObjectContext _objectContext;
        private Dictionary<string, object> _repositories;
        private DbTransaction _transaction;

        #endregion Private Fields

        #region Public Properties

        public IDataContext DbContext { get { return _dataContext; } }

        #endregion

        #region Constuctor/Dispose

        public UnitOfWork(IDataContextAsync dataContext)
        {
            _dataContext = dataContext;
        }

        public void Dispose()
        {
            if (_objectContext != null && _objectContext.Connection.State == ConnectionState.Open)
                _objectContext.Connection.Close();

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _dataContext.Dispose();
            _disposed = true;
        }

        #endregion Constuctor/Dispose

        public int SaveChanges()
        {
            return _dataContext.SaveChanges();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : Entity
        {
            return RepositoryAsync<TEntity>();
        }

        public Task<int> SaveChangesAsync()
        {
            return _dataContext.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _dataContext.SaveChangesAsync(cancellationToken);
        }

        public IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : Entity
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
                return (IRepositoryAsync<TEntity>)_repositories[type];

            var repositoryType = typeof(Repository<>);
            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dataContext));

            return (IRepositoryAsync<TEntity>)_repositories[type];
        }

        #region Unit of Work Transactions

        public void BeginTransaction()
        {
            _objectContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
                _transaction = _objectContext.Connection.BeginTransaction();
            }
        }

        public bool Commit()
        {
            _transaction.Commit();
            _objectContext.Connection.Close();
            _objectContext.Connection.Dispose();
            return true;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            ((DataContext.DataContext)_dataContext).SyncObjectsStatePostCommit();
        }

        #endregion

        // Uncomment, if rather have IRepositoryAsync<TEntity> IoC vs. Reflection Activation
        //public IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : EntityBase
        //{
        //    return ServiceLocator.Current.GetInstance<IRepositoryAsync<TEntity>>();
        //}
    }
}