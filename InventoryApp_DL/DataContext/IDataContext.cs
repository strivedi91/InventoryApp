using System;
using System.Data.Entity;

namespace InventoryApp_DL.DataContext
{
    public interface IDataContext : IDisposable
    {
        DbContext DbContext { get; }
        
        DbSet<T> Set<T>() where T : class;

        int SaveChanges();
        
        void SyncObjectState(object entity);
    }
}