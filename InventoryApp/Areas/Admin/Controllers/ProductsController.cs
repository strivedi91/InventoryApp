using InventoryApp.Areas.Admin.Models;
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
    public class ProductsController : Controller
    {
        public ActionResult Index()
        {
            ProductsViewModel foRequest = new ProductsViewModel();
            foRequest.stSortColumn = "ID ASC";
            return View("~/Areas/Admin/Views/Products/Index.cshtml", getProductsList(foRequest));
        }

        public ActionResult searchProducts(ProductsViewModel foSearchRequest)
        {
            ProductsModel loProductsModel = getProductsList(foSearchRequest);
            return PartialView("~/Areas/Admin/Views/Products/_Products.cshtml", loProductsModel);
        }

        public ProductsModel getProductsList(ProductsViewModel foRequest)
        {
            if (foRequest.inPageSize <= 0)
                foRequest.inPageSize = 10;

            if (foRequest.inPageIndex <= 0)
                foRequest.inPageIndex = 1;

            if (foRequest.stSortColumn == "")
                foRequest.stSortColumn = null;

            if (foRequest.stSearch == "")
                foRequest.stSearch = null;


            Func<IQueryable<Products>, IOrderedQueryable<Products>> orderingFunc =
            query => query.OrderBy(x => x.id);

            Expression<Func<Products, bool>> expression = null;

            if (!string.IsNullOrEmpty(foRequest.stSearch))
            {
                foRequest.stSearch = foRequest.stSearch.Replace("%20", " ");
                expression = x => x.Name.ToLower().Contains(foRequest.stSearch.ToLower()) && x.IsDeleted == false;
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
                    case "Name DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Name);
                        break;
                    case "Name ASC":
                        orderingFunc = q => q.OrderBy(s => s.Name);
                        break;
                    case "Description DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Description);
                        break;
                    case "Description ASC":
                        orderingFunc = q => q.OrderBy(s => s.Description);
                        break;
                    case "Brand DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Brand);
                        break;
                    case "Brand ASC":
                        orderingFunc = q => q.OrderBy(s => s.Brand);
                        break;
                    case "Type DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Type);
                        break;
                    case "Type ASC":
                        orderingFunc = q => q.OrderBy(s => s.Type);
                        break;
                    case "Price DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Price);
                        break;
                    case "Price ASC":
                        orderingFunc = q => q.OrderBy(s => s.Price);
                        break;
                    case "OfferPrice DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.OfferPrice);
                        break;
                    case "OfferPrice ASC":
                        orderingFunc = q => q.OrderBy(s => s.OfferPrice);
                        break;
                    case "Quantity DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Quantity);
                        break;
                    case "Quantity ASC":
                        orderingFunc = q => q.OrderBy(s => s.Quantity);
                        break;
                    case "MOQ DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.MOQ);
                        break;
                    case "MOQ ASC":
                        orderingFunc = q => q.OrderBy(s => s.MOQ);
                        break;
                    case "CategoryId DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.CategoryId);
                        break;
                    case "CategoryId ASC":
                        orderingFunc = q => q.OrderBy(s => s.CategoryId);
                        break;
                }
            }

            (List<Products>, int) objProducts = Repository<Products>.GetEntityListForQuery(expression, orderingFunc, null, foRequest.inPageIndex, foRequest.inPageSize);

            ProductsModel objProductsViewModel = new ProductsModel();

            objProductsViewModel.inRecordCount = objProducts.Item2;
            objProductsViewModel.inPageIndex = foRequest.inPageIndex;
            objProductsViewModel.Pager = new Pager(objProducts.Item2, foRequest.inPageIndex);

            if (objProducts.Item1.Count > 0)
            {
                foreach (var prod in objProducts.Item1)
                {
                    objProductsViewModel.loProductList.Add(new ProductsViewModel
                    {
                        id = prod.id,
                        Name = prod.Name,
                        Description = prod.Description,
                        Type = prod.Type,
                        Brand = prod.Brand,
                        Price = prod.Price,
                        OfferPrice = prod.OfferPrice,
                        Quantity = prod.Quantity,
                        MOQ = prod.MOQ,
                        CategoryId = prod.CategoryId,
                        CategoryName = Repository<Categories>.GetEntityListForQuery(x => x.Id == prod.CategoryId).Item1.Select(x => x.Name).FirstOrDefault(),
                        IsActive = prod.IsActive
                    });
                }
            }

            return objProductsViewModel;
        }

        public ActionResult AddEditProducts()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddEditProduct(int? id)
        {
            ProductsViewModel objProductsViewModel = new ProductsViewModel();

            var objCategories = Repository<Categories>.GetEntityListForQuery(x => x.IsDeleted == false).Item1.ToList();

            objProductsViewModel.objCategoryList.Add(new SelectListItem { Text = "-- Select --", Value = "0", Selected = true });

            foreach (var Category in objCategories)
            {
                objProductsViewModel.objCategoryList.Add(new SelectListItem { Text = Category.Name, Value = Category.Id.ToString(), Selected = false });
            }

            if (id != null && id > 0)
            {
                Products objProduct = Repository<Products>.GetEntityListForQuery(x => x.id == id).Item1.FirstOrDefault();

                objProductsViewModel = new ProductsViewModel
                {
                    id = objProduct.id,
                    Name = objProduct.Name,
                    Description = objProduct.Description,
                    Type = objProduct.Type,
                    Brand = objProduct.Brand,
                    Price = objProduct.Price,
                    OfferPrice = objProduct.OfferPrice,
                    Quantity = objProduct.Quantity,
                    MOQ = objProduct.MOQ,
                    CategoryId = objProduct.CategoryId,
                    IsActive = objProduct.IsActive,
                };
            }

            return View("~/Areas/Admin/Views/Products/AddEditProduct.cshtml", objProductsViewModel);
        }

        [ValidateInput(false)]
        [HttpPost]
        public async Task<ActionResult> AddEditProduct(ProductsViewModel foProduct)
        {
            if (foProduct != null)
            {
                try
                {
                    if (foProduct.id == 0)
                    {
                        Products objProduct = new Products();
                        objProduct.Name = foProduct.Name;
                        objProduct.Description = foProduct.Description;
                        objProduct.Type = foProduct.Type;
                        objProduct.Brand = foProduct.Brand;
                        objProduct.Price = foProduct.Price.GetValueOrDefault();
                        objProduct.OfferPrice = foProduct.OfferPrice;
                        objProduct.Quantity = foProduct.Quantity.GetValueOrDefault();
                        objProduct.MOQ = foProduct.MOQ;
                        objProduct.CategoryId = foProduct.CategoryId.GetValueOrDefault();
                        objProduct.IsActive = foProduct.IsActive;
                        await Repository<Products>.InsertEntity(objProduct, entity => { return entity.id; });

                        TempData["SuccessMsg"] = "Product has been added successfully";
                    }
                    else
                    {
                        Products objProduct = Repository<Products>.GetEntityListForQuery(x => x.id == foProduct.id).Item1.FirstOrDefault();
                        objProduct.Name = foProduct.Name;
                        objProduct.Description = foProduct.Description;
                        objProduct.Type = foProduct.Type;
                        objProduct.Brand = foProduct.Brand;
                        objProduct.Price = foProduct.Price.GetValueOrDefault();
                        objProduct.OfferPrice = foProduct.OfferPrice;
                        objProduct.Quantity = foProduct.Quantity.GetValueOrDefault();
                        objProduct.MOQ = foProduct.MOQ;
                        objProduct.CategoryId = foProduct.CategoryId.GetValueOrDefault();
                        objProduct.IsActive = foProduct.IsActive;
                        await Repository<Products>.UpdateEntity(objProduct, (entity) => { return entity.id; });

                        TempData["SuccessMsg"] = "Product has been updated successfully";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMsg"] = "Something wrong!! Please try after sometime";
                }
            }
            return RedirectToAction("Index", "Products");
        }

        public ActionResult DeleteProduct(int Id)
        {
            int liSuccess = 0;
            string lsMessage = string.Empty;

            if (Id > 0)
            {
                Products objProducts = Repository<Products>.GetEntityListForQuery(x => x.id == Id).Item1.FirstOrDefault();
                objProducts.IsDeleted = true;
                Repository<Products>.UpdateEntity(objProducts, (entity) => { return entity.id; });

                TempData["SuccessMsg"] = "Products has been deleted successfully";
            }

            return this.Json(new ProductsViewModel { id = liSuccess });
        }
    }
}