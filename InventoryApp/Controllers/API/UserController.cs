﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using InventoryApp.Models;
using InventoryApp.Models.ApiModels;
using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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

                var UserPreferences = Repository<AspNetUserPreferences>.GetEntityListForQuery(x => x.UserId == LoggedInUserId);

                Repository<AspNetUserPreferences>.DeleteRange(UserPreferences.Item1);

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

                Result = JObject.FromObject(new
                {
                    status = true,
                    message = "Preferences Saved Successfully !",
                    Result = ""
                });

                return GetOkResult(Result);
            }
            catch (Exception ex)
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Sorry, there was an error processing your request. Please try again !",
                    SavePreferencesResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("user/getpreferences")]
        public async Task<IHttpActionResult> GetUserPreferences()
        {
            JObject Result = null;

            try
            {


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
                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "",
                        PreferenceResult = JObject.FromObject(new
                        {
                            FirstTimeLogin = false,
                            Categories =
                               from category in userCategories
                               select new
                               {
                                   Id = category.Categories.Id,
                                   Name = category.Categories.Name,
                                   ProductCount = Repository<Products>.GetEntityListForQuery(x => x.CategoryId == category.CategoryId && x.IsActive == true).Item1.Select(x => x.id).Count(),
                                   SelectedProductCount = userCategories.Where(x => x.CategoryId == category.CategoryId).Count()
                               }
                        })
                    });
                }
                else
                {
                    List<Expression<Func<Categories, Object>>> includesforProduct = new List<Expression<Func<Categories, object>>>();
                    Expression<Func<Categories, object>> IncludeProduct = (product) => product.Products;
                    includesforProduct.Add(IncludeProduct);
                    var categories = Repository<Categories>.GetEntityListForQuery(x => x.IsActive == true && x.IsDeleted == false, null, includesforProduct).Item1;
                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "",
                        PreferenceResult = JObject.FromObject(new
                        {
                            FirstTimeLogin = true,
                            Categories =
                                    from category in categories
                                    select new
                                    {
                                        Id = category.Id,
                                        Name = category.Name,
                                        ProductCount = category.Products.Where(x => x.IsActive == true).Count(),
                                        SelectedProductCount = 0
                                    }
                        })
                    });
                }
                return GetOkResult(Result);
            }
            catch (Exception ex)
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Sorry, there was an error processing your request. Please try again !",
                    PreferenceResult = ""
                });
                return GetOkResult(Result);
            }
        }


        [HttpGet]
        [Route("user/products")]
        public async Task<IHttpActionResult> GetProducts()
        {
            JObject loJObjResult = null;

            try
            {
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
                    message = "",
                    ProductResult = JObject.FromObject(new
                    {

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
                    })
                });
                return GetOkResult(loJObjResult);
            }
            catch (Exception)
            {
                loJObjResult = JObject.FromObject(new
                {
                    status = false,
                    message = "Sorry, there was an error processing your request. Please try again !",
                    ProductResult = ""
                });
                return GetOkResult(loJObjResult);
            }
        }

        [HttpGet]
        [Route("category/{Id:int}/products")]
        public async Task<IHttpActionResult> GetProductsByCategoryId(int Id)
        {
            var LoggedinUser = User.Identity.GetUserId();
            JObject loJObjResult = null;
            try
            {

                List<Expression<Func<Products, Object>>> includes = new List<Expression<Func<Products, object>>>();
                Expression<Func<Products, object>> IncludeCategories = (category) => category.Categories;
                Expression<Func<Products, object>> IncludeTierPricing = (pricing) => pricing.TierPricings;
                Expression<Func<Products, object>> IncludeUserPreferences = (userPreferences) => userPreferences.AspNetUserPreferences;
                includes.Add(IncludeCategories);
                includes.Add(IncludeTierPricing);
                includes.Add(IncludeUserPreferences);

                var products = Repository<Products>.GetEntityListForQuery(x => x.IsActive && x.CategoryId == Id, null, includes).Item1;

                loJObjResult = JObject.FromObject(new
                {
                    status = true,
                    message = "",
                    ProductResult = JObject.FromObject(new
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
                           TierPricing = product.TierPricings.Select(x => new { x.QtyTo, x.QtyFrom, x.Price }),
                           selected = product.AspNetUserPreferences.Where(x => x.UserId == LoggedinUser).Count() >= 1 ? true : false
                       }
                    })
                });
                return GetOkResult(loJObjResult);
            }
            catch (Exception)
            {
                loJObjResult = JObject.FromObject(new
                {
                    status = false,
                    message = "Sorry, there was an error processing your request. Please try again !",
                    ProductResult = ""
                });
                return GetOkResult(loJObjResult);
            }
        }

        [HttpPost]
        [Route("user/addtocart")]
        public async Task<IHttpActionResult> AddToCart(AddToCartModel[] addToCartModel)
        {
            JObject Result = null;

            try
            {
                var LoggedInUserId = User.Identity.GetUserId();

                List<Cart> cartItems = new List<Cart>();
                cartItems.AddRange(
                    addToCartModel.
                    Select(x => new Cart
                    {
                        UserId = LoggedInUserId,
                        OfferId = x.OfferId,
                        Quantity = x.Quantity,
                        ProductId = x.ProductId
                    }));

                await Repository<Cart>.InsertMultipleEntities(cartItems);

                Result = JObject.FromObject(new
                {
                    status = true,
                    message = "Item added to cart !",
                    Result = ""
                });

                return GetOkResult(Result);
            }
            catch (Exception ex)
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Sorry, there was an error processing your request. Please try again !",
                    Result = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("user/cart")]
        public async Task<IHttpActionResult> GetCart()
        {
            JObject Result = null;

            try
            {
                var LoggedInUserId = User.Identity.GetUserId();

                List<Expression<Func<Cart, Object>>> includes = new List<Expression<Func<Cart, object>>>();
                Expression<Func<Cart, object>> IncludeProducts = (product) => product.Products;
                Expression<Func<Cart, object>> IncludeCategory = (category) => category.Categories;
                includes.Add(IncludeProducts);
                includes.Add(IncludeCategory);

                var userSelectedProducts = Repository<Cart>.
                    GetEntityListForQuery(x => x.UserId == LoggedInUserId, null, includes).Item1;

                Result = JObject.FromObject(new
                {
                    status = true,
                    message = "",
                    Result = JObject.FromObject(new
                    {

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
                                SelectedQuantity = product.Quantity,
                                TierPricing = Repository<TierPricing>.GetEntityListForQuery(x => x.ProductId == product.Products.id && x.IsActive).
                                Item1.Select(x => new { x.QtyTo, x.QtyFrom, x.Price })
                            }
                    })
                });
                return GetOkResult(Result);
            }
            catch (Exception ex)
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Sorry, there was an error processing your request. Please try again !",
                    Result = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpPost]
        [Route("user/placeorder")]
        public async Task<IHttpActionResult> PlaceOrder(OrderModel orderModel)
        {
            JObject Result = null;

            try
            {
                var LoggedInUserId = User.Identity.GetUserId();

                Orders orders = new Orders
                {
                    CreatedOn = DateTime.Now,
                    Discount = orderModel.OrderDiscount,
                    OrderStatus = "Order Placed Successfully.",
                    SubTotal = orderModel.OrderSubTotal,
                    Total = orderModel.OrderTotal,
                    UserId = LoggedInUserId
                };

                await Repository<Orders>.InsertEntity(orders, entity => { return entity.id; });

                List<OrderDetails> orderItems = new List<OrderDetails>();
                orderItems.AddRange(
                    orderModel.orderDetailsModels.
                    Select(x => new OrderDetails
                    {
                        OrderId = orders.id,
                        Quantity = x.Quantity,
                        CategoryId = x.CategoryId,
                        Discount = x.Discount,
                        Price = x.Price,
                        TotalPrice = x.TotalPrice,
                        ProductId = x.ProductId
                    }));

                await Repository<OrderDetails>.InsertMultipleEntities(orderItems);

                Result = JObject.FromObject(new
                {
                    status = true,
                    message = "Order placed successfully !",
                    Result = ""
                });
                return GetOkResult(Result);
            }
            catch (Exception ex)
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Sorry, there was an error processing your request. Please try again !",
                    Result = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("user/orders")]
        public async Task<IHttpActionResult> GetOrders()
        {
            JObject Result = null;

            try
            {
                var LoggedInUserId = User.Identity.GetUserId();

                List<Expression<Func<Orders, Object>>> includes = new List<Expression<Func<Orders, object>>>();
                Expression<Func<Orders, object>> IncludeOrderDetails = (orderdetails) => orderdetails.OrderDetails;
                includes.Add(IncludeOrderDetails);

                var userOrders = Repository<Orders>.
                    GetEntityListForQuery(x => x.UserId == LoggedInUserId, null, includes).Item1;

                Result = JObject.FromObject(new
                {
                    status = true,
                    message = "",
                    Result = JObject.FromObject(new
                    {

                        Orders =
                            from order in userOrders
                            select new
                            {
                                Id = order.id,
                                Products = order.OrderDetails.Count(),
                                order.CreatedOn,
                                order.Discount,
                                order.OrderStatus,
                                order.SubTotal,
                                order.Total
                            }
                    })
                });
                return GetOkResult(Result);
            }
            catch (Exception ex)
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Sorry, there was an error processing your request. Please try again !",
                    Result = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("user/orders/{orderId:int}/details")]
        public async Task<IHttpActionResult> GetOrderDetails(int orderId)
        {
            JObject Result = null;

            try
            {
                var LoggedInUserId = User.Identity.GetUserId();

                List<Expression<Func<OrderDetails, Object>>> includes = new List<Expression<Func<OrderDetails, object>>>();
                Expression<Func<OrderDetails, object>> IncludeProducts = (orderdetails) => orderdetails.Products;
                Expression<Func<OrderDetails, object>> IncludeCategories = (orderdetails) => orderdetails.Categories;
                includes.Add(IncludeProducts);
                includes.Add(IncludeCategories);

                var userOrders = Repository<OrderDetails>.
                    GetEntityListForQuery(x => x.OrderId == orderId, null, includes).Item1;

                Result = JObject.FromObject(new
                {
                    status = true,
                    message = "",
                    Result = JObject.FromObject(new
                    {
                        Products =
                        from product in userOrders
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
                    })
                });
                return GetOkResult(Result);
            }
            catch (Exception ex)
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Sorry, there was an error processing your request. Please try again !",
                    Result = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IHttpActionResult> LoginUser(LoginViewModel model)
        {
            JObject Result = null;
            try
            {
                var request = HttpContext.Current.Request;
                var tokenServiceUrl = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "/Authenticate";
                using (var client = new HttpClient())
                {
                    var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Email),
                new KeyValuePair<string, string>("password", model.Password)
            };
                    var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                    var tokenServiceResponse = await client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded);
                    var responseString = await tokenServiceResponse.Content.ReadAsStringAsync();
                    var authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseString.ToString());

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "Authentication Successfull !",
                        LoginResult = authenticationResponse
                    });
                }
                return GetOkResult(Result);

            }
            catch (Exception)
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Invalid Login Attempt, Please try correct username and password !",
                    LoginResult = ""
                });
                return GetOkResult(Result);
            }
        }

    }
}
