using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp_DL.Infrastructure
{
    [Serializable]
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; } //TODO: Renamed since a possible coflict with State entity column
    }
}