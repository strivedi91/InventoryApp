using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using InventoryApp.Models.ApiModels;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace InventoryApp.Controllers.API
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : BaseAPIController
    {
        [HttpPost]
        [Route("savepreferences")]
        public async Task<IHttpActionResult> SaveUserPreferences(SaveUserPreferencesModel[] saveUserPreferencesModel)
        {
            JObject Result = null;

            try
            {
                var LoggedInUserId = User.Identity.GetUserId();

                List<AspNetUserPreferences> listUserCategories = new List<AspNetUserPreferences>();
                listUserCategories.AddRange(
                    saveUserPreferencesModel.
                    Select(x => new AspNetUserPreferences
                    {
                        UserId = LoggedInUserId,
                        CategoryId = x.CategoryId,
                        ProductId = x.ProductId
                    }));

                await Repository<AspNetUserPreferences>.InsertMultipleEntities(listUserCategories);

                Result = new JObject(
                        new JProperty("message", "User Preferences Stored Successfully."),
                        new JProperty("status", true)
                    );

                return GetOkResult(Result);
            }
            catch (Exception ex)
            {
                return GetInternalServerErrorResult("Sorry, there was an error processing your request. Please try again.");
            }
        }

        [HttpGet]
        [Route("getpreferences")]
        public async Task<IHttpActionResult> GetUserPreferences()
        {
            JObject loJObjResult = null;

            List<Expression<Func<AspNetUserPreferences, Object>>> includes = new List<Expression<Func<AspNetUserPreferences, object>>>();
            Expression<Func<AspNetUserPreferences, object>> IncludeCategories = (category) => category.Categories;
            Expression<Func<AspNetUserPreferences, object>> IncludeProducts = (product) => product.Products;
            includes.Add(IncludeCategories);
            includes.Add(IncludeProducts);

            string LoggedInUserId = User.Identity.GetUserId();
            var userCategories = Repository<AspNetUserPreferences>.GetEntityListForQuery(x => x.UserId == LoggedInUserId, null, includes).Item1;

            if (userCategories.Count() > 0)
            {
                loJObjResult = JObject.FromObject(new
                {
                    Categories =
                       from category in userCategories
                       .GroupBy(x => x.id)
                       .Select(x => x.FirstOrDefault())
                       select new
                       {
                           Id = category.Categories.Id,
                           Name = category.Categories.Name,
                           ProductCount = Repository<Products>.GetEntityListForQuery(x => x.CategoryId == category.CategoryId && x.IsActive == true).Item1.Select(x => x.id).Count(),
                           SelectedProductCount = userCategories.Where(x => x.CategoryId == category.CategoryId).Count()
                       }
                });
            }
            else
            {
                List<Expression<Func<Categories, Object>>> includesforProduct = new List<Expression<Func<Categories, object>>>();
                Expression<Func<Categories, object>> IncludeProduct = (product) => product.Products;
                includesforProduct.Add(IncludeProduct);
                var categories = Repository<Categories>.GetEntityListForQuery(x => x.IsActive == true, null, includesforProduct).Item1;

               loJObjResult = JObject.FromObject(new
                {
                    Categories =
                       from category in categories
                       select new
                       {
                           Id = category.Id,
                           Name = category.Name,
                           ProductCount = category.Products.Where(x => x.IsActive == true).Count(),
                           SelectedProductCount = 0
                       }
                });
            }

            return GetOkResult(loJObjResult);
        }
    }
}
