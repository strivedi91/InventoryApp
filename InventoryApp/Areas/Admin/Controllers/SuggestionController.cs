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
    public class SuggestionController : Controller
    {
        public ActionResult Index()
        {
            SuggestionsViewModel foRequest = new SuggestionsViewModel();

            foRequest.stSortColumn = "ID ASC";

            return View(getSuggestionList(foRequest));
        }

        public ActionResult searchSuggestions(SuggestionsViewModel foSearchRequest)
        {
            SuggestionsModel loSuggestionModel = getSuggestionList(foSearchRequest);

            return PartialView("~/Areas/Admin/Views/Suggestion/_Suggestions.cshtml", loSuggestionModel);
        }

        public SuggestionsModel getSuggestionList(SuggestionsViewModel foRequest)
        {
            if (foRequest.inPageSize <= 0)
                foRequest.inPageSize = 10;

            if (foRequest.inPageIndex <= 0)
                foRequest.inPageIndex = 1;

            if (foRequest.stSortColumn == "")
                foRequest.stSortColumn = null;

            if (foRequest.stSearch == "")
                foRequest.stSearch = null;


            List<Expression<Func<Suggestions, Object>>> includes = new List<Expression<Func<Suggestions, object>>>();
            Expression<Func<Suggestions, object>> IncludeProducts = (product) => product.Products;
            Expression<Func<Suggestions, object>> IncludeUser = (suggestion) => suggestion.AspNetUsers;
            includes.Add(IncludeProducts);
            includes.Add(IncludeUser);

            Func<IQueryable<Suggestions>, IOrderedQueryable<Suggestions>> orderingFunc =
            query => query.OrderBy(x => x.Id);

            Expression<Func<Suggestions, bool>> expression = null;

            //expression = x => x.IsDeleted == false;

            if (!string.IsNullOrEmpty(foRequest.stSortColumn))
            {
                switch (foRequest.stSortColumn)
                {
                    case "ProductName DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.ProductId);
                        break;
                    case "ProductName ASC":
                        orderingFunc = q => q.OrderBy(s => s.ProductId);
                        break;
                    case "UserName DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.AspNetUsers.Name);
                        break;
                    case "UserName ASC":
                        orderingFunc = q => q.OrderBy(s => s.AspNetUsers.Name);
                        break;
                    case "UserContact DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.AspNetUsers.PhoneNumber);
                        break;
                    case "UserContact ASC":
                        orderingFunc = q => q.OrderBy(s => s.AspNetUsers.PhoneNumber);
                        break;
                    case "Suggestion DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Suggestion);
                        break;
                    case "Suggestion ASC":
                        orderingFunc = q => q.OrderBy(s => s.Suggestion);
                        break;
                }
            }
            (List<Suggestions>, int) objSuggestions = Repository<Suggestions>.GetEntityListForQuery(expression, orderingFunc, includes, foRequest.inPageIndex, foRequest.inPageSize);

            SuggestionsModel objProductsViewModel = new SuggestionsModel();

            objProductsViewModel.inRecordCount = objSuggestions.Item2;
            objProductsViewModel.inPageIndex = foRequest.inPageIndex;
            objProductsViewModel.Pager = new Pager(objSuggestions.Item2, foRequest.inPageIndex);

            if (objSuggestions.Item1.Count > 0)
            {
                foreach (var suggestion in objSuggestions.Item1)
                {
                    objProductsViewModel.loSuggestionList.Add(new SuggestionsViewModel
                    {
                        Id = suggestion.Id,
                        ProductName = suggestion.Products.Name,
                        UserName = suggestion.AspNetUsers.Name,
                        UserContectNumber = suggestion.AspNetUsers.PhoneNumber,
                        Suggestion = suggestion.Suggestion
                    });
                }
            }

            return objProductsViewModel;
        }
    }
}