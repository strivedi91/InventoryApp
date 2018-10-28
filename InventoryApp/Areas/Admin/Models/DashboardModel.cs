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
        public int TodayOrderCount { get; set; }
        public int YesterdayOrderCount { get; set; }
        public int CurrentMonthOrderCount { get; set; }
        public int LastMonthOrderCount { get; set; }

        public int CancelledOrderCount { get; set; }
        public int CancelledTodayOrderCount { get; set; }
        public int CancelledYesterdayOrderCount { get; set; }
        public int CancelledCurrentMonthOrderCount { get; set; }
        public int CancelledLastMonthOrderCount { get; set; }

        public decimal TodayOrderPayment { get; set; }
        public decimal YesterdayOrderPayment { get; set; }
        public decimal CurrentMonthOrderPayment { get; set; }
        public decimal LastMonthOrderPayment { get; set; }
        public decimal TotalPayment { get; set; }
    }
}