using InventoryApp.Helpers;
using InventoryApp_DL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Models
{
    public class PageAccessModel
    {
        public PageAccessModel()
        {
            objUserList = new List<SelectListItem>();
        }

        public int Id { get; set; }

        public int PageId { get; set; }

        public string PageName { get; set; }

        public string UserId { get; set; }

        public bool? IsAccessGranted { get; set; }

        public List<SelectListItem> objUserList { get; set; }
    }

    public class PageAccessList
    {
        public List<PageAccessModel> objPageAccess { get; set; }
    }
}