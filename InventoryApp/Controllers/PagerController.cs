using InventoryApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Controllers
{
    public class PagerController : Controller
    {
        [HttpPost]
        public ActionResult getPagerData(int RecCount, int PageIndex)
        {

            //return View(new Pager(RecCount, PageIndex));
            return PartialView("~/Views/Shared/_Pager.cshtml", new Pager(RecCount, PageIndex));
        }
    }
}