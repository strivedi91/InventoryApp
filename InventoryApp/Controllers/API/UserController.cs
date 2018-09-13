using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IHttpActionResult> SaveUserPreferences(SaveUserPreferencesModel saveUserPreferencesModel)
        {
            JObject Result = null;

            try
            {                
                var LoggedInUserId = User.Identity.GetUserId();

                List<AspNetUserCategories> listUserCategories = new List<AspNetUserCategories>();
                listUserCategories.AddRange(
                    saveUserPreferencesModel.Categories.
                    Select(x => new AspNetUserCategories
                    {
                        UserId = LoggedInUserId,
                        CategoryId = x
                    }));    
               
               await Repository<AspNetUserCategories>.InsertMultipleEntities(listUserCategories);

                List<AspNetUserProducts> listUserProducts = new List<AspNetUserProducts>();
                listUserProducts.AddRange(
                    saveUserPreferencesModel.Products.
                    Select(x => new AspNetUserProducts
                    {
                        UserId = LoggedInUserId,
                        ProductId = x
                    }));

              await  Repository<AspNetUserProducts>.InsertMultipleEntities(listUserProducts);

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
    }
}
