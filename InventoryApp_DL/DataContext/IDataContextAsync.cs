using System.Threading;
using System.Threading.Tasks;

namespace InventoryApp_DL.DataContext
{
    public interface IDataContextAsync : IDataContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
    }
}