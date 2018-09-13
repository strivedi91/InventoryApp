using System;
using InventoryApp_DL.Repositories;
using InventoryApp_DL.DataContext;

namespace InventoryApp_DL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : Infrastructure.Entity;
        void BeginTransaction();
        bool Commit();
        void Rollback();

        IDataContext DbContext { get; }
    }
}