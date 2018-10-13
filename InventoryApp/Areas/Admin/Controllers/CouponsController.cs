using InventoryApp.Areas.Admin.Models;
using InventoryApp.Controllers;
using InventoryApp.Helpers;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CouponsController : BaseController
    {
        public ActionResult Index()
        {
            OffersViewModel foRequest = new OffersViewModel();
            foRequest.stSortColumn = "ID ASC";

            return View("~/Areas/Admin/Views/Coupons/Index.cshtml", getCouponsList(foRequest));
        }

        public ActionResult searchCoupons(OffersViewModel foSearchRequest)
        {
            OffersModel loCouponsModel = getCouponsList(foSearchRequest);
            return PartialView("~/Areas/Admin/Views/Coupons/_Coupons.cshtml", loCouponsModel);
        }

        public OffersModel getCouponsList(OffersViewModel foRequest)
        {
            if (foRequest.inPageSize <= 0)
                foRequest.inPageSize = 10;

            if (foRequest.inPageIndex <= 0)
                foRequest.inPageIndex = 1;

            if (foRequest.stSortColumn == "")
                foRequest.stSortColumn = null;

            if (foRequest.stSearch == "")
                foRequest.stSearch = null;


            Func<IQueryable<Offers>, IOrderedQueryable<Offers>> orderingFunc =
            query => query.OrderBy(x => x.id);

            Expression<Func<Offers, bool>> expression = null;

            if (!string.IsNullOrEmpty(foRequest.stSearch))
            {
                foRequest.stSearch = foRequest.stSearch.Replace("%20", " ");
                expression = x => x.OfferCode.ToLower().Contains(foRequest.stSearch.ToLower()) && x.IsDeleted == false;
            }
            else
                expression = x => x.IsDeleted == false;

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
                    case "OfferCode DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.OfferCode);
                        break;
                    case "OfferCode ASC":
                        orderingFunc = q => q.OrderBy(s => s.OfferCode);
                        break;
                    case "OfferDescription DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.OfferDescription);
                        break;
                    case "OfferDescription ASC":
                        orderingFunc = q => q.OrderBy(s => s.OfferDescription);
                        break;
                    case "FlatDiscount DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.FlatDiscount);
                        break;
                    case "FlatDiscount ASC":
                        orderingFunc = q => q.OrderBy(s => s.FlatDiscount);
                        break;
                    case "PercentageDiscount DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.PercentageDiscount);
                        break;
                    case "PercentageDiscount ASC":
                        orderingFunc = q => q.OrderBy(s => s.PercentageDiscount);
                        break;
                    case "ProductId DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.ProductId);
                        break;
                    case "ProductId ASC":
                        orderingFunc = q => q.OrderBy(s => s.ProductId);
                        break;
                    case "CategoryId DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.CategoryId);
                        break;
                    case "CategoryId ASC":
                        orderingFunc = q => q.OrderBy(s => s.CategoryId);
                        break;
                    case "StartDate DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.StartDate);
                        break;
                    case "StartDate ASC":
                        orderingFunc = q => q.OrderBy(s => s.StartDate);
                        break;
                    case "EndDate DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.EndDate);
                        break;
                    case "EndDate ASC":
                        orderingFunc = q => q.OrderBy(s => s.EndDate);
                        break;
                    case "IsActive DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.IsActive);
                        break;
                    case "IsActive ASC":
                        orderingFunc = q => q.OrderBy(s => s.IsActive);
                        break;
                }
            }

            (List<Offers>, int) objCoupons = Repository<Offers>.GetEntityListForQuery(expression, orderingFunc, null, foRequest.inPageIndex, foRequest.inPageSize);

            OffersModel objCouponViewModel = new OffersModel();

            objCouponViewModel.inRecordCount = objCoupons.Item2;
            objCouponViewModel.inPageIndex = foRequest.inPageIndex;
            objCouponViewModel.Pager = new Pager(objCoupons.Item2, foRequest.inPageIndex);

            //#region Category DDl
            //var objCategories = Repository<Categories>.GetEntityListForQuery(x => x.IsDeleted == false).Item1.ToList();

            //objProductsViewModel.objCategoryList.Add(new SelectListItem { Text = "-- Select --", Value = "0", Selected = true });

            //foreach (var Category in objCategories)
            //{
            //    objProductsViewModel.objCategoryList.Add(new SelectListItem { Text = Category.Name, Value = Category.Id.ToString(), Selected = false });
            //}
            //#endregion

            if (objCoupons.Item1.Count > 0)
            {
                foreach (var coupon in objCoupons.Item1)
                {
                    objCouponViewModel.loOfferList.Add(new OffersViewModel
                    {
                        id = coupon.id,
                        OfferCode = coupon.OfferCode,
                        OfferDescription = coupon.OfferDescription,
                        FlatDiscount = coupon.FlatDiscount,
                        PercentageDiscount = coupon.PercentageDiscount,
                        ProductId = coupon.ProductId,
                        ProductName = Repository<Products>.GetEntityListForQuery(x => x.id == coupon.ProductId).Item1.Select(x => x.Name).FirstOrDefault(),
                        CategoryId = coupon.CategoryId,
                        StartDate = coupon.StartDate,
                        EndDate = coupon.EndDate,
                        CategoryName = Repository<Categories>.GetEntityListForQuery(x => x.Id == coupon.CategoryId).Item1.Select(x => x.Name).FirstOrDefault(),
                        IsActive = coupon.IsActive
                    });
                }
            }

            return objCouponViewModel;
        }

        [HttpGet]
        public ActionResult AddEditCoupon(int? id)
        {
            OffersViewModel objCouponDetails = new OffersViewModel();

            if (id != null && id > 0)
            {
                Offers objOffers = Repository<Offers>.GetEntityListForQuery(x => x.id == id).Item1.FirstOrDefault();

                objCouponDetails = new OffersViewModel
                {
                    id = objOffers.id,
                    OfferCode = objOffers.OfferCode,
                    OfferDescription = objOffers.OfferDescription,
                    FlatDiscount = objOffers.FlatDiscount,
                    PercentageDiscount = objOffers.PercentageDiscount,
                    ProductId = objOffers.ProductId,
                    CategoryId = objOffers.CategoryId,
                    IsActive = objOffers.IsActive,
                    StartDate = objOffers.StartDate,
                    EndDate = objOffers.EndDate
                };
            }

            #region Product
            var objProducts = Repository<Products>.GetEntityListForQuery(x => x.IsDeleted == false).Item1.ToList();

            objCouponDetails.objProductList.Add(new SelectListItem { Text = "-- Select Product --", Value = "0", Selected = true });

            foreach (var product in objProducts)
            {
                objCouponDetails.objProductList.Add(new SelectListItem { Text = product.Name, Value = product.id.ToString(), Selected = false });
            }
            #endregion

            #region Category
            var objCategories = Repository<Categories>.GetEntityListForQuery(x => x.IsDeleted == false).Item1.ToList();

            objCouponDetails.objCategoryList.Add(new SelectListItem { Text = "-- Select Category --", Value = "0", Selected = true });

            foreach (var Category in objCategories)
            {
                objCouponDetails.objCategoryList.Add(new SelectListItem { Text = Category.Name, Value = Category.Id.ToString(), Selected = false });
            }
            #endregion

            return View("~/Areas/Admin/Views/Coupons/AddEditCoupon.cshtml", objCouponDetails);
        }


        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> AddEditCoupon(OffersViewModel foCouponDetails)
        {
            if (foCouponDetails != null)
            {
                try
                {
                    if (foCouponDetails.id == 0)
                    {
                        Offers objOffers = new Offers()
                        {
                            OfferCode = foCouponDetails.OfferCode,
                            OfferDescription = foCouponDetails.OfferDescription,
                            FlatDiscount = foCouponDetails.FlatDiscount.GetValueOrDefault(0),
                            PercentageDiscount = foCouponDetails.PercentageDiscount.GetValueOrDefault(0),
                            ProductId = foCouponDetails.ProductId == 0 ? null : foCouponDetails.ProductId,
                            CategoryId = foCouponDetails.CategoryId == 0 ? null : foCouponDetails.CategoryId,
                            IsActive = foCouponDetails.IsActive,
                            StartDate = foCouponDetails.StartDate,
                            EndDate = foCouponDetails.EndDate,
                        };

                        await Repository<Offers>.InsertEntity(objOffers, entity => { return entity.id; });

                        TempData["SuccessMsg"] = "Coupon has been added successfully";
                    }
                    else
                    {
                        Offers objOffers = Repository<Offers>.GetEntityListForQuery(x => x.id == foCouponDetails.id).Item1.FirstOrDefault();

                        objOffers.OfferCode = foCouponDetails.OfferCode;
                        objOffers.OfferDescription = foCouponDetails.OfferDescription;
                        objOffers.FlatDiscount = foCouponDetails.FlatDiscount.GetValueOrDefault(0);
                        objOffers.PercentageDiscount = foCouponDetails.PercentageDiscount.GetValueOrDefault(0);
                        objOffers.ProductId = foCouponDetails.ProductId == 0 ? null : foCouponDetails.ProductId;
                        objOffers.CategoryId = foCouponDetails.CategoryId == 0 ? null : foCouponDetails.CategoryId;
                        objOffers.IsActive = foCouponDetails.IsActive;
                        objOffers.StartDate = foCouponDetails.StartDate;
                        objOffers.EndDate = foCouponDetails.EndDate;
                        await Repository<Offers>.UpdateEntity(objOffers, (entity) => { return entity.id; });

                        TempData["SuccessMsg"] = "Coupon has been updated successfully";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMsg"] = "Something wrong!! Please try after sometime";
                }
            }
            return RedirectToAction("Index", "Coupons");
        }

        public async Task<ActionResult> DeleteCoupon(int Id)
        {
            int liSuccess = 0;
            string lsMessage = string.Empty;

            if (Id > 0)
            {
                try
                {
                    Offers objOffers = Repository<Offers>.GetEntityListForQuery(x => x.id == Id).Item1.FirstOrDefault();
                    objOffers.IsDeleted = true;
                    await Repository<Offers>.UpdateEntity(objOffers, (entity) => { return entity.id; });

                    TempData["SuccessMsg"] = "Coupon has been deleted successfully";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMsg"] = "Something wrong!! Please try after sometime";
                }
            }

            return this.Json(new OffersViewModel { id = liSuccess });
        }
    }
}