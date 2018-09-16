using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryApp.Areas.Admin.Models
{
    public class DashboardModel
    {
        public int SellerCount { get; set; }
        public int ProductCount { get; set; }
        public int CategoryCount { get; set; }
        public int OrderCount { get; set; }
    }
}