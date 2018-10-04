using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Controllers
{
    public class SuggestionController : Controller
    {
        // GET: Admin/Suggestion
        public ActionResult Index()
        {
            return View();
        }
    }
}