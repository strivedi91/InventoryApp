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
    public class SuggestionController : BaseController
    {
        public ActionResult Index()
        {
            SuggestionsViewModel foRequest = new SuggestionsViewModel();

            foRequest.stSortColumn = "ID DESC";

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

                    case "ID DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.Id);
                        break;
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
                    case "UserEmail DESC":
                        orderingFunc = q => q.OrderByDescending(s => s.AspNetUsers.Email);
                        break;
                    case "UserEmail ASC":
                        orderingFunc = q => q.OrderBy(s => s.AspNetUsers.Email);
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

            SuggestionsModel objSuggestion = new SuggestionsModel();

            objSuggestion.inRecordCount = objSuggestions.Item2;
            objSuggestion.inPageIndex = foRequest.inPageIndex;
            objSuggestion.Pager = new Pager(objSuggestions.Item2, foRequest.inPageIndex);

            if (objSuggestions.Item1.Count > 0)
            {
                foreach (var suggestion in objSuggestions.Item1)
                {
                    objSuggestion.loSuggestionList.Add(new SuggestionsViewModel
                    {
                        Id = suggestion.Id,
                        ProductName = suggestion.Products.Name,
                        UserName = suggestion.AspNetUsers.Name,
                        Email = suggestion.AspNetUsers.Email,
                        UserContectNumber = suggestion.AspNetUsers.PhoneNumber,
                        Suggestion = suggestion.Suggestion,
                        IsReplied = suggestion.IsReplied,
                        ProductImgPath = getProductImgPath(suggestion.ProductId)
                    });
                }
            }

            return objSuggestion;
        }

        [HttpPost]
        public async Task<JsonResult> ReplayForSuggestion(SuggestionReplayRequest foRequest)
        {
            bool IsSuccess = false;

            List<Expression<Func<Suggestions, Object>>> includes = new List<Expression<Func<Suggestions, object>>>();
            Expression<Func<Suggestions, object>> IncludeProducts = (product) => product.Products;
            Expression<Func<Suggestions, object>> IncludeUser = (suggestion) => suggestion.AspNetUsers;
            includes.Add(IncludeProducts);
            includes.Add(IncludeUser);

            Expression<Func<Suggestions, bool>> expression = x => x.Id == foRequest.SuggestionId;

            var objSuggestions = Repository<Suggestions>.GetEntityListForQuery(expression, null, includes).Item1.FirstOrDefault();

            objSuggestions.SuggestionResponse = foRequest.Replay;
            objSuggestions.IsReplied = true;

            await Repository<Suggestions>.UpdateEntity(objSuggestions, (entity) => { return entity.Id; });

            sendEmailReplayForSuggestion(objSuggestions);

            return Json(IsSuccess);
        }

        #region Private Methodes
        private string getProductImgPath(int foProductId)
        {
            string stProductImgPath = string.Empty;
            string ProductImagePath = ConfigurationManager.AppSettings["ProductImagePath"].ToString();
            string path = Path.Combine(Server.MapPath(ProductImagePath), foProductId.ToString());

            List<SelectListItem> fileNameWithPath = new List<SelectListItem>();
            if (Directory.Exists(path))
            {
                DirectoryInfo info = new DirectoryInfo(path);
                FileInfo[] files = info.GetFiles("*.*");

                if (files != null && files.Count() > 0)
                    stProductImgPath = ConfigurationManager.AppSettings["ProductImagePath"].Replace("~", "") + "/" + foProductId.ToString() + "/" + files[0].Name;
            }

            return stProductImgPath;
        }

        private bool sendEmailReplayForSuggestion(Suggestions foSuggestions)
        {
            try
            {
                string lsFrom = "no-replay@thgodowninventorryapp.com";

                string lsToMails = foSuggestions.AspNetUsers.Email;

                string lsSubject = string.Empty;

                string lsEmailBody = string.Empty;

                lsSubject = "Replay for your suggestion";

                lsEmailBody = "Hi " + foSuggestions.AspNetUsers.Name + ",<br /><br />";

                lsEmailBody += "<b>Your Suggestion:</b><br/>";

                lsEmailBody += foSuggestions.Suggestion;

                lsEmailBody += "<br /><br />";

                lsEmailBody += "<b>Replay:</b><br/>";

                lsEmailBody += foSuggestions.SuggestionResponse;

                lsEmailBody += "<br /><br />";

                lsEmailBody += "<p>Best Regards, <br /> The Inventiry Team<br /> " + ConfigurationManager.AppSettings["SiteAddress"] + "</p>";

                return EmailHelper.sendEmail(lsToMails, lsFrom, lsSubject, lsEmailBody);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}