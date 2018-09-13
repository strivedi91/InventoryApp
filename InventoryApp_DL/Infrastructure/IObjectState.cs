
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp_DL.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}