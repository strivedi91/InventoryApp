using InventoryApp.Areas.Admin.Models;
using InventoryApp.Helpers;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            OrderViewModel foRequest = new OrderViewModel();
            foRequest.stSortColumn = "ID ASC";
            return View("~/Areas/Admin/Views/Order/Index.cshtml", getOrderList(foRequest)); 
        }

        public ActionResult searchProducts(OrderViewModel foSearchRequest)
        {
            OrderModel loProductsModel = getOrderList(foSearchRequest);
            return PartialView("~/Areas/Admin/Views/Order/_Order.cshtml", loProductsModel);
        }
        public OrderModel getOrderList(OrderViewModel foRequest)
        {
            if (foRequest.inPageSize <= 0)
                foRequest.inPageSize = 10;

            if (foRequest.inPageIndex <= 0)
                foRequest.inPageIndex = 1;

            if (foRequest.stSortColumn == "")
                foRequest.stSortColumn = null;

            if (foRequest.stSearch == "")
                foRequest.stSearch = null;


            Func<IQueryable<Orders>, IOrderedQueryable<Orders>> orderingFunc =
            query => query.OrderBy(x => x.id);

            Expression<Func<Orders, bool>> expression = null;

            List<Expression<Func<Orders, Object>>> includes = new List<Expression<Func<Orders, object>>>();
            Expression<Func<Orders, object>> IncludeProducts = (orderdetails) => orderdetails.OrderDetails;            
            includes.Add(IncludeProducts);
            

            //if (!string.IsNullOrEmpty(foRequest.stSearch))
            //{
            //    foRequest.stSearch = foRequest.stSearch.Replace("%20", " ");
            //    expression = x => x.id == Convert.ToInt32 foRequest.stSearch;
            //}
            //else
            //    expression = x => x.IsDeleted == false;

            if (!string.IsNullOrEmpty(foRequest.stSortColumn))
            {
                switch (foRequest.stSortColumn)
                {
                    case "ID DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.id);
                        break;
                    case "ID ASC":
                        orderingFunc = q => q.OrderBy(s => s.id);
                        break;
                    case "CreatedOn DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.CreatedOn);
                        break;
                    case "CreatedOn ASC":
                        orderingFunc = q => q.OrderBy(s => s.CreatedOn);
                        break;
                    case "Discount DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Discount);
                        break;
                    case "Discount ASC":
                        orderingFunc = q => q.OrderBy(s => s.Discount);
                        break;
                    case "OrderStatus DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.OrderStatus);
                        break;
                    case "OrderStatus ASC":
                        orderingFunc = q => q.OrderBy(s => s.OrderStatus);
                        break;
                    case "SubTotal DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.SubTotal);
                        break;
                    case "SubTotal ASC":
                        orderingFunc = q => q.OrderBy(s => s.SubTotal);
                        break;
                    case "Total DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Total);
                        break;
                    case "Total ASC":
                        orderingFunc = q => q.OrderBy(s => s.Total);
                        break;                    
                }
            }

            (List<Orders>, int) objProducts = Repository<Orders>.GetEntityListForQuery(expression, orderingFunc, includes, foRequest.inPageIndex, foRequest.inPageSize);

            OrderModel objOrderViewModel = new OrderModel();

            objOrderViewModel.inRecordCount = objProducts.Item2;
            objOrderViewModel.inPageIndex = foRequest.inPageIndex;
            objOrderViewModel.Pager = new Pager(objProducts.Item2, foRequest.inPageIndex);

            if (objProducts.Item1.Count > 0)
            {
                foreach (var order in objProducts.Item1)
                {
                    objOrderViewModel.loOrderList.Add(new OrderViewModel
                    {
                        id = order.id,
                        ProductCount = order.OrderDetails.Count(),
                        CreatedOn = order.CreatedOn,
                        Discount = order.Discount,
                        OrderStatus = order.OrderStatus,
                        SubTotal = order.SubTotal,
                        Total = order.Total
                    });
                }
            }

            return objOrderViewModel;
        }

    }
}