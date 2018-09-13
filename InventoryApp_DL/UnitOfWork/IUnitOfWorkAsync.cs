#region

using System.Threading;
using System.Threading.Tasks;
using InventoryApp_DL.Infrastructure;
using InventoryApp_DL.Repositories;

#endregion

namespace InventoryApp_DL.Interfaces
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : Entity;
    }
}