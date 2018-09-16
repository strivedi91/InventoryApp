using InventoryApp.Areas.Admin.Models;
using InventoryApp.Models;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            (List<AspNetUsers>, int) objUsers = Repository<AspNetUsers>.GetEntityListForQuery(null);
            (List<Products>, int) objProducts = Repository<Products>.GetEntityListForQuery(null);
            (List<Categories>, int) objCategories = Repository<Categories>.GetEntityListForQuery(null);
            (List<Orders>, int) objOrders = Repository<Orders>.GetEntityListForQuery(null);

            return View(new DashboardModel
            {
                CategoryCount = objCategories.Item2,
                OrderCount = objOrders.Item2,
                ProductCount = objProducts.Item2,
                SellerCount = objUsers.Item2
            });
        }
    }
}