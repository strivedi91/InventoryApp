using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryApp.Models.ApiModels
{
    public class CancelOrderModel
    {
        public int OrderId { get; set; }
        public string CancelReason { get; set; }
    }
}