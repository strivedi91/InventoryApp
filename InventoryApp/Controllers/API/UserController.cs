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
    [RoutePrefix("api")]
    public class UserController : BaseAPIController
    {
        [HttpPost]
        [Route("user/savepreferences")]
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
        [Route("user/getpreferences")]
        public async Task<IHttpActionResult> GetUserPreferences()
        {
            JObject loJObjResult = null;

            List<Expression<Func<AspNetUserPreferences, Object>>> includes = new List<Expression<Func<AspNetUserPreferences, object>>>();
            Expression<Func<AspNetUserPreferences, object>> IncludeCategories = (category) => category.Categories;
            Expression<Func<AspNetUserPreferences, object>> IncludeProducts = (product) => product.Products;
            includes.Add(IncludeCategories);
            includes.Add(IncludeProducts);

            string LoggedInUserId = User.Identity.GetUserId();
            var userCategories = Repository<AspNetUserPreferences>.GetEntityListForQuery(x => x.UserId == LoggedInUserId, null, includes).
                Item1.GroupBy(x => x.CategoryId)
                       .Select(x => x.FirstOrDefault());

            if (userCategories.Count() > 0)
            {
                loJObjResult = JObject.FromObject(new
                {
                    Categories =
                       from category in userCategories

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
                    status = true,
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


        [HttpGet]
        [Route("user/products")]
        public async Task<IHttpActionResult> GetProducts()
        {
            JObject loJObjResult = null;

            List<Expression<Func<AspNetUserPreferences, Object>>> includes = new List<Expression<Func<AspNetUserPreferences, object>>>();
            Expression<Func<AspNetUserPreferences, object>> IncludeCategories = (category) => category.Categories;
            Expression<Func<AspNetUserPreferences, object>> IncludeProducts = (product) => product.Products;
            includes.Add(IncludeCategories);
            includes.Add(IncludeProducts);

            string LoggedInUserId = User.Identity.GetUserId();
            var userSelectedProducts = Repository<AspNetUserPreferences>.
                GetEntityListForQuery(x => x.UserId == LoggedInUserId, null, includes).Item1;

            loJObjResult = JObject.FromObject(new
            {
                status = true,
                Products =
                        from product in userSelectedProducts
                        select new
                        {
                            Id = product.Products.id,
                            Name = product.Products.Name,
                            Category = product.Categories.Name,
                            Brand = product.Products.Brand,
                            Description = product.Products.Description,
                            Type = product.Products.Type,
                            Price = product.Products.Price,
                            OfferPrice = product.Products.OfferPrice,
                            product.Products.MOQ,
                            product.Products.Quantity,
                            TierPricing = Repository<TierPricing>.GetEntityListForQuery(x => x.ProductId == product.Products.id && x.IsActive).
                            Item1.Select(x => new { x.QtyTo, x.QtyFrom, x.Price })
                        }
            });

            return GetOkResult(loJObjResult);
        }

        [HttpGet]
        [Route("category/{Id:int}/products")]
        public async Task<IHttpActionResult> GetProductsByCategoryId(int Id)
        {
            JObject loJObjResult = null;

            List<Expression<Func<Products, Object>>> includes = new List<Expression<Func<Products, object>>>();
            Expression<Func<Products, object>> IncludeCategories = (category) => category.Categories;
            Expression<Func<Products, object>> IncludeTierPricing = (pricing) => pricing.TierPricings;
            includes.Add(IncludeCategories);
            includes.Add(IncludeTierPricing);

            var products = Repository<Products>.GetEntityListForQuery(x => x.IsActive && x.CategoryId == Id, null, includes).Item1;

            loJObjResult = JObject.FromObject(new
            {
                Products =
                   from product in products
                   select new
                   {
                       Id = product.id,
                       Name = product.Name,
                       Category = product.Categories.Name,
                       Brand = product.Brand,
                       Description = product.Description,
                       Type = product.Type,
                       Price = product.Price,
                       OfferPrice = product.OfferPrice,
                       product.MOQ,
                       product.Quantity,
                       TierPricing = product.TierPricings.Select(x => new { x.QtyTo, x.QtyFrom, x.Price })
                   }
            });
            return GetOkResult(loJObjResult);
        }

        [HttpPost]
        [Route("addtocart")]
        public async Task<IHttpActionResult> AddToCart()
        {

        }
    }
}
