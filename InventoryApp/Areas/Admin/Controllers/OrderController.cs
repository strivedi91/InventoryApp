using InventoryApp.Areas.Admin.Models;
using InventoryApp.Controllers;
using InventoryApp.Helpers;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            OrderViewModel foRequest = new OrderViewModel();
            foRequest.stSortColumn = "ID DESC";
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

            DateTime? ldFromDate = null;
            if (!string.IsNullOrEmpty(foRequest.lsFromDate))
                ldFromDate = Convert.ToDateTime(foRequest.lsFromDate + " 00:00:01");

            DateTime? ldToDate = null;
            if (!string.IsNullOrEmpty(foRequest.lsToDate))
                ldToDate = Convert.ToDateTime(foRequest.lsToDate + " 23:59:59");

            // 1. Seller, from date, to date, category
            if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsFromDate) && !string.IsNullOrEmpty(foRequest.lsToDate) && !string.IsNullOrEmpty(foRequest.OrderStatus) && foRequest.inFilterCategory > 0)
                expression = x => x.CreatedOn >= ldFromDate && x.CreatedOn <= ldToDate && x.UserId == foRequest.inFilterSeller && x.OrderStatus == foRequest.OrderStatus && (x.OrderDetails.Where(y=>y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 1. Seller, from date, to date, category
            if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsFromDate) && !string.IsNullOrEmpty(foRequest.lsToDate) && foRequest.inFilterCategory > 0)
                expression = x => x.CreatedOn >= ldFromDate && x.CreatedOn <= ldToDate && x.UserId == foRequest.inFilterSeller && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 2. Seller, from date, to date
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsFromDate) && !string.IsNullOrEmpty(foRequest.lsToDate) && !string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.CreatedOn >= ldFromDate && x.CreatedOn <= ldToDate && x.UserId == foRequest.inFilterSeller && x.OrderStatus == foRequest.OrderStatus && x.OrderDetails.Count() > 0;

            // 2. Seller, from date, to date
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsFromDate) && !string.IsNullOrEmpty(foRequest.lsToDate))
                expression = x => x.CreatedOn >= ldFromDate && x.CreatedOn <= ldToDate && x.UserId == foRequest.inFilterSeller && x.OrderDetails.Count() > 0;

            // 3. from date, to date, category
            else if (!string.IsNullOrEmpty(foRequest.lsFromDate) && !string.IsNullOrEmpty(foRequest.lsToDate) && foRequest.inFilterCategory > 0 && !string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.CreatedOn >= ldFromDate && x.CreatedOn <= ldToDate && x.OrderStatus == foRequest.OrderStatus && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1);

            // 3. from date, to date, category
            else if (!string.IsNullOrEmpty(foRequest.lsFromDate) && !string.IsNullOrEmpty(foRequest.lsToDate) && foRequest.inFilterCategory > 0)
                expression = x => x.CreatedOn >= ldFromDate && x.CreatedOn <= ldToDate && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 4. Seller, from date, category
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsFromDate) && foRequest.inFilterCategory > 0 && !string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.CreatedOn >= ldFromDate && x.UserId == foRequest.inFilterSeller && x.OrderStatus == foRequest.OrderStatus && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 4. Seller, from date, category
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsFromDate) && foRequest.inFilterCategory > 0)
                expression = x => x.CreatedOn >= ldFromDate && x.UserId == foRequest.inFilterSeller && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 5. Seller, to date, category
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsToDate) && foRequest.inFilterCategory > 0 && !string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.CreatedOn <= ldToDate && x.UserId == foRequest.inFilterSeller && x.OrderStatus == foRequest.OrderStatus && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 5. Seller, to date, category
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsToDate) && foRequest.inFilterCategory > 0)
                expression = x => x.CreatedOn <= ldToDate && x.UserId == foRequest.inFilterSeller && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 6. Seller, category
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && foRequest.inFilterCategory > 0 && !string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.UserId == foRequest.inFilterSeller && x.OrderStatus == foRequest.OrderStatus && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 6. Seller, category
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && foRequest.inFilterCategory > 0)
                expression = x => x.UserId == foRequest.inFilterSeller && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 7. from date, to date
            else if (!string.IsNullOrEmpty(foRequest.lsFromDate) && !string.IsNullOrEmpty(foRequest.lsToDate) && !string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.CreatedOn >= ldFromDate && x.CreatedOn <= ldToDate && x.OrderStatus == foRequest.OrderStatus && x.OrderDetails.Count() > 0;

            // 7. from date, to date
            else if (!string.IsNullOrEmpty(foRequest.lsFromDate) && !string.IsNullOrEmpty(foRequest.lsToDate))
                expression = x => x.CreatedOn >= ldFromDate && x.CreatedOn <= ldToDate && x.OrderDetails.Count() > 0;

            // 8. Seller, from date
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsFromDate) && !string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.CreatedOn >= ldFromDate && x.UserId == foRequest.inFilterSeller && x.OrderStatus == foRequest.OrderStatus && x.OrderDetails.Count() > 0;

            // 8. Seller, from date
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsFromDate))
                expression = x => x.CreatedOn >= ldFromDate && x.UserId == foRequest.inFilterSeller && x.OrderDetails.Count() > 0;

            // 9. Seller, to date
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsToDate) && !string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.CreatedOn <= ldToDate && x.UserId == foRequest.inFilterSeller && x.OrderStatus == foRequest.OrderStatus && x.OrderDetails.Count() > 0;

            // 9. Seller, to date
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller) && !string.IsNullOrEmpty(foRequest.lsToDate))
                expression = x => x.CreatedOn <= ldToDate && x.UserId == foRequest.inFilterSeller && x.OrderDetails.Count() > 0;

            // 10. Category, from date
            else if (foRequest.inFilterCategory > 0 && !string.IsNullOrEmpty(foRequest.lsFromDate) && !string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.CreatedOn >= ldFromDate && x.OrderStatus == foRequest.OrderStatus && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 10. Category, from date
            else if (foRequest.inFilterCategory > 0 && !string.IsNullOrEmpty(foRequest.lsFromDate))
                expression = x => x.CreatedOn >= ldFromDate && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 11. Category, to date
            else if (foRequest.inFilterCategory > 0 && !string.IsNullOrEmpty(foRequest.lsToDate) && !string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.CreatedOn <= ldToDate && x.OrderStatus == foRequest.OrderStatus && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 11. Category, to date
            else if (foRequest.inFilterCategory > 0 && !string.IsNullOrEmpty(foRequest.lsToDate))
                expression = x => x.CreatedOn <= ldToDate && (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 12. Seller
            else if (!string.IsNullOrEmpty(foRequest.inFilterSeller))
                expression = x => x.UserId == foRequest.inFilterSeller && x.OrderDetails.Count() > 0;

            // 13. Category
            else if (foRequest.inFilterCategory > 0)
                expression = x => (x.OrderDetails.Where(y => y.CategoryId == foRequest.inFilterCategory).Count() >= 1) && x.OrderDetails.Count() > 0;

            // 14. From date
            else if (!string.IsNullOrEmpty(foRequest.lsFromDate))
                expression = x => x.CreatedOn >= ldFromDate && x.OrderDetails.Count() > 0;

            // 15. To date
            else if (!string.IsNullOrEmpty(foRequest.lsToDate))
                expression = x => x.CreatedOn <= ldToDate && x.OrderDetails.Count() > 0;

            // 15. To Status
            else if (!string.IsNullOrEmpty(foRequest.OrderStatus))
                expression = x => x.OrderStatus == foRequest.OrderStatus && x.OrderDetails.Count() > 0;
            else
                expression = x=> x.OrderDetails.Count() > 0;

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
                    default:
                        orderingFunc = q => q.OrderByDescending(s => s.CreatedOn);
                        break;
                }
            }

            (List<Orders>, int) objProducts = Repository<Orders>.GetEntityListForQuery(expression, orderingFunc, includes, foRequest.inPageIndex, foRequest.inPageSize);

            OrderModel objOrderViewModel = new OrderModel();

            objOrderViewModel.inRecordCount = objProducts.Item2;
            objOrderViewModel.inPageIndex = foRequest.inPageIndex;
            objOrderViewModel.Pager = new Pager(objProducts.Item2, foRequest.inPageIndex);
                        
            #region Category DDl
            var objCategories = Repository<Categories>.GetEntityListForQuery(x => x.IsDeleted == false).Item1.ToList();

            objOrderViewModel.loCategoryList.Add(new SelectListItem { Text = "ALL", Value = "0", Selected = true });

            foreach (var Category in objCategories)
            {
                objOrderViewModel.loCategoryList.Add(new SelectListItem { Text = Category.Name, Value = Category.Id.ToString(), Selected = false });
            }
            #endregion

            #region Seller DDl
            var objSeller = Repository<AspNetUsers>.GetEntityListForQuery(x => x.IsDeleted == false).Item1.ToList();
            
            objOrderViewModel.loSellerList.Add(new SelectListItem { Text = "ALL", Value = "", Selected = true });

            foreach (var user in objSeller)
            {   
                objOrderViewModel.loSellerList.Add(new SelectListItem { Text = user.Name, Value = user.Id.ToString(), Selected = false });
            }
            #endregion

            #region Order Status DDl
            objOrderViewModel.loOrdeStatusList.Add(new SelectListItem { Text = Enums.GetEnumDescription((Enums.OrderStatus.OrderPlaced)), Value = Enums.GetEnumDescription((Enums.OrderStatus.OrderPlaced)) });
            objOrderViewModel.loOrdeStatusList.Add(new SelectListItem { Text = Enums.GetEnumDescription((Enums.OrderStatus.InProcess)), Value = Enums.GetEnumDescription((Enums.OrderStatus.InProcess)) });
            objOrderViewModel.loOrdeStatusList.Add(new SelectListItem { Text = Enums.GetEnumDescription((Enums.OrderStatus.Dispatched)), Value = Enums.GetEnumDescription((Enums.OrderStatus.Dispatched)) });
            objOrderViewModel.loOrdeStatusList.Add(new SelectListItem { Text = Enums.GetEnumDescription((Enums.OrderStatus.DeliveredCompleted)), Value = Enums.GetEnumDescription((Enums.OrderStatus.DeliveredCompleted)) });
            objOrderViewModel.loOrdeStatusList.Add(new SelectListItem { Text = Enums.GetEnumDescription((Enums.OrderStatus.Cancelled)), Value = Enums.GetEnumDescription((Enums.OrderStatus.Cancelled)) });
            #endregion

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


        public async Task<JsonResult> updateOrderStatus(int fiOrderId, string fsOrderStatus)
        {
            bool flgIsSuccess = false;
            try
            {
                Orders objOrder = Repository<Orders>.GetEntityListForQuery(x => x.id == fiOrderId).Item1.FirstOrDefault();
                objOrder.OrderStatus = fsOrderStatus;
                await Repository<Orders>.UpdateEntity(objOrder, (entity) => { return entity.id; });
                flgIsSuccess = true;
            }
            catch
            {
                flgIsSuccess = false;
            }

            return Json(flgIsSuccess, JsonRequestBehavior.AllowGet);
        }

    }
}