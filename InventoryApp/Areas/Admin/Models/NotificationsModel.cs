using InventoryApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryApp.Areas.Admin.Models
{
    public class NotificationsModel
    {
        public NotificationsModel()
        {
            notifications = new List<NotificaitonViewModel>();
        }

        public Int64 inRecordCount { get; set; }
        public int inPageIndex { get; set; }
        public Pager Pager { get; set; }

        public List<NotificaitonViewModel> notifications { get; set; }
    }

    public class NotificaitonViewModel
    {
        public int Id { get; set; }
        public string NotificaitonText { get; set; }
        public DateTime CreatedOn { get; set; }
    }

}