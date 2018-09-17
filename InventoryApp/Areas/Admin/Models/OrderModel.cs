using InventoryApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Models
{
    public class OrderModel
    {
        public OrderModel()
        {
            loOrderList = new List<OrderViewModel>();
            loOrdeStatusList = new List<SelectListItem>();
        }

        public Int64 inRecordCount { get; set; }
        public int inPageIndex { get; set; }
        public Pager Pager { get; set; }

        public List<SelectListItem> loOrdeStatusList { get; set; }

        public List<OrderViewModel> loOrderList { get; set; }
    }

    public class OrderViewModel
    {
        public int id { get; set; }

        public int ProductCount { get; set; } 

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal Discount { get; set; }
        
        public string OrderStatus { get; set; }

        public decimal SubTotal { get; set; }
                
        public decimal  Total { get; set; }

        public string stSearch { get; set; }

        public int inPageIndex { get; set; }

        public int inPageSize { get; set; }

        public string stSortColumn { get; set; }

        public Int64 inRowNumber { get; set; }
    }
}