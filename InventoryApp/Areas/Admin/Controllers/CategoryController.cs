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
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            CategoryViewModel foRequest = new CategoryViewModel();
            foRequest.stSortColumn = "ID ASC";
            return View("~/Areas/Admin/Views/Category/Index.cshtml", getCategoryList(foRequest));
        }

        public ActionResult searchCategory(CategoryViewModel foSearchRequest)
        {
            CategoryModel loCategoryModel = getCategoryList(foSearchRequest);
            return PartialView("~/Areas/Admin/Views/Category/_Category.cshtml", loCategoryModel);
        }

        public CategoryModel getCategoryList(CategoryViewModel foRequest)
        {
            if (foRequest.inPageSize <= 0)
                foRequest.inPageSize = 10;

            if (foRequest.inPageIndex <= 0)
                foRequest.inPageIndex = 1;

            if (foRequest.stSortColumn == "")
                foRequest.stSortColumn = null;

            if (foRequest.stSearch == "")
                foRequest.stSearch = null;


            Func<IQueryable<Categories>, IOrderedQueryable<Categories>> orderingFunc =
            query => query.OrderBy(x => x.Id);

            Expression<Func<Categories, bool>> expression = null;

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
                        orderingFunc = q => q.OrderByDescending(s => s.Id);
                        break;
                    case "ID ASC":
                        orderingFunc = q => q.OrderBy(s => s.Id);
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
                }
            }

            (List<Categories>, int) objUsers = Repository<Categories>.GetEntityListForQuery(expression, orderingFunc, null, foRequest.inPageIndex, foRequest.inPageSize);


            CategoryModel objCategoryViewModel = new CategoryModel();

            objCategoryViewModel.inRecordCount = objUsers.Item2;
            objCategoryViewModel.inPageIndex = foRequest.inPageIndex;
            objCategoryViewModel.Pager = new Pager(objUsers.Item2, foRequest.inPageIndex);

            if (objUsers.Item1.Count > 0)
            {
                foreach (var catg in objUsers.Item1)
                {
                    objCategoryViewModel.loCategoryList.Add(new CategoryViewModel
                    {
                        Id = catg.Id,
                        Name = catg.Name,
                        Description = catg.Description,
                        IsActive = catg.IsActive
                    });
                }
            }

            return objCategoryViewModel;
        }

        public ActionResult AddEditCategory()
        {
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddEditCategory(CategoryViewModel loCategoryViewModel)
        {
            if (loCategoryViewModel != null)
            {
                try
                {
                    if (loCategoryViewModel.Id == 0)
                    {
                        Categories lowebstats = new Categories();
                        lowebstats.Name = loCategoryViewModel.Name;
                        lowebstats.Description = loCategoryViewModel.Description;
                        lowebstats.IsActive = loCategoryViewModel.IsActive;
                        Repository<Categories>.InsertEntity(lowebstats, entity => { return entity.Id; });
                                                
                        TempData["SuccessMsg"] = "Category has been added successfully";
                    }
                    else
                    {
                        Categories lowebstats = Repository<Categories>.GetEntityListForQuery(x => x.Id == loCategoryViewModel.Id).Item1.FirstOrDefault();
                        lowebstats.Name = loCategoryViewModel.Name;
                        lowebstats.Description = loCategoryViewModel.Description;
                        lowebstats.IsActive = loCategoryViewModel.IsActive;                        
                        Repository<Categories>.UpdateEntity(lowebstats, (entity) => { return entity.Id; });

                        TempData["SuccessMsg"] = "Category has been updated successfully";
                    }
                }
                catch (Exception)
                {
                    TempData["ErrorMsg"] = "Something wrong!! Please try after sometime";
                }
            }
            return RedirectToAction("Index", "Category");
        }
        [HttpGet]
        public ActionResult AddEditCategory(int? id)
        {
            CategoryViewModel objCategoryViewModel = new CategoryViewModel();

            if (id != null && id > 0)
            {
                Categories objPackages = Repository<Categories>.GetEntityListForQuery(x => x.Id == id).Item1.FirstOrDefault();

                objCategoryViewModel = new CategoryViewModel
                {
                    Id = objPackages.Id,
                    Name = objPackages.Name,
                    Description = objPackages.Description
                };
            }

            return View("~/Areas/Admin/Views/Category/AddEditCategory.cshtml", objCategoryViewModel);
        }
        public ActionResult DeleteCategory(int ID)
        {
            int liSuccess = 0;
            string lsMessage = string.Empty;

            if (ID > 0)
            {
                Categories objPackages = Repository<Categories>.GetEntityListForQuery(x => x.Id == ID).Item1.FirstOrDefault();                
                objPackages.IsDeleted = true;
                Repository<Categories>.UpdateEntity(objPackages, (entity) => { return entity.Id; });

                TempData["SuccessMsg"] = "Category has been deleted successfully";
            }

            return this.Json(new CategoryViewModel { Id = liSuccess });
        }
    }
}