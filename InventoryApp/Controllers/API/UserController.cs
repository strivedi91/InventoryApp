using System;
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
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InventoryApp.Controllers.API
{
    [RoutePrefix("api")]
    public class UserController : BaseAPIController
    {

        [HttpGet]
        [Route("categories")]
        public async Task<IHttpActionResult> getAllCategories()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
<<<<<<< HEAD
                try
                {
                    List<Expression<Func<Categories, Object>>> includesforProduct = new List<Expression<Func<Categories, object>>>();
                    Expression<Func<Categories, object>> IncludeProduct = (product) => product.Products;
                    includesforProduct.Add(IncludeProduct);
                    var categories = Repository<Categories>.GetEntityListForQuery(x => x.IsActive == true && x.IsDeleted == false, null, includesforProduct).Item1;
                    Result = JObject.FromObject(new
=======
                var LoggedInUserId = User.Identity.GetUserId();

                var UserPreferences = Repository<AspNetUserPreferences>.GetEntityListForQuery(x => x.UserId == LoggedInUserId);

                Repository<AspNetUserPreferences>.DeleteRange(UserPreferences.Item1);

                List<AspNetUserPreferences> listUserCategories = new List<AspNetUserPreferences>();
                listUserCategories.AddRange(
                    saveUserPreferencesModel.
                    Select(x => new AspNetUserPreferences
>>>>>>> 22fdf8d34de0c710184aaa802c209dbb235b213d
                    {
                        status = true,
                        message = "",
                        CategoryResult = JObject.FromObject(new
                        {
                            Categories =
                                    from category in categories
                                    select new
                                    {
                                        Id = category.Id,
                                        Name = category.Name,
                                        ProductCount = category.Products.Where(x => x.IsActive == true).Count(),
                                        SelectedProductCount = Repository<AspNetUserPreferences>.GetEntityListForQuery(x => x.CategoryId == category.Id && x.UserId == LoggedInUserId).Item1.Count()
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
                        CategoryResult = ""
                    });
                    return GetOkResult(Result);
                }
            }
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    CategoryResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpPost]
        [Route("user/savepreferences")]
        public async Task<IHttpActionResult> SaveUserPreferences(SavePreferencesModels saveUserPreferencesModel)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    List<AspNetUserPreferences> listUserCategories = new List<AspNetUserPreferences>();
                    listUserCategories.AddRange(
                        saveUserPreferencesModel.SavePreferences.
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
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    SavePreferencesResult = ""
                });
                return GetOkResult(Result);
            }
        }


        [HttpGet]
        [Route("user/getpreferences")]
        public async Task<IHttpActionResult> GetUserPreferences()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    List<Expression<Func<AspNetUserPreferences, Object>>> includes = new List<Expression<Func<AspNetUserPreferences, object>>>();
                    Expression<Func<AspNetUserPreferences, object>> IncludeCategories = (category) => category.Categories;
                    Expression<Func<AspNetUserPreferences, object>> IncludeProducts = (product) => product.Products;
                    includes.Add(IncludeCategories);
                    includes.Add(IncludeProducts);

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
                                       SelectedProductCount = Repository<AspNetUserPreferences>.GetEntityListForQuery(x => x.CategoryId == category.CategoryId).Item1.Count()
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
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    PreferenceResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("user/category/{Id:int}/products")]
        public async Task<IHttpActionResult> GetProducts(int Id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    List<Expression<Func<AspNetUserPreferences, Object>>> includes = new List<Expression<Func<AspNetUserPreferences, object>>>();
                    Expression<Func<AspNetUserPreferences, object>> IncludeCategories = (category) => category.Categories;
                    Expression<Func<AspNetUserPreferences, object>> IncludeProducts = (product) => product.Products;
                    includes.Add(IncludeCategories);
                    includes.Add(IncludeProducts);


                    var userSelectedProducts = Repository<AspNetUserPreferences>.
                        GetEntityListForQuery(x => x.UserId == LoggedInUserId && x.CategoryId == Id, null, includes).Item1;

                    Result = JObject.FromObject(new
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
                    return GetOkResult(Result);
                }
                catch (Exception)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        ProductResult = ""
                    });
                    return GetOkResult(Result);
                }
            }
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    ProductResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("category/{Id:int}/products")]
        public async Task<IHttpActionResult> GetProductsByCategoryId(int Id)
        {
<<<<<<< HEAD
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
=======
            var LoggedinUser = User.Identity.GetUserId();
            JObject loJObjResult = null;
            try
>>>>>>> 22fdf8d34de0c710184aaa802c209dbb235b213d
            {
                try
                {

<<<<<<< HEAD
                    List<Expression<Func<Products, Object>>> includes = new List<Expression<Func<Products, object>>>();
                    Expression<Func<Products, object>> IncludeCategories = (category) => category.Categories;
                    Expression<Func<Products, object>> IncludeTierPricing = (pricing) => pricing.TierPricings;
                    includes.Add(IncludeCategories);
                    includes.Add(IncludeTierPricing);
=======
                List<Expression<Func<Products, Object>>> includes = new List<Expression<Func<Products, object>>>();
                Expression<Func<Products, object>> IncludeCategories = (category) => category.Categories;
                Expression<Func<Products, object>> IncludeTierPricing = (pricing) => pricing.TierPricings;
                Expression<Func<Products, object>> IncludeUserPreferences = (userPreferences) => userPreferences.AspNetUserPreferences;
                includes.Add(IncludeCategories);
                includes.Add(IncludeTierPricing);
                includes.Add(IncludeUserPreferences);
>>>>>>> 22fdf8d34de0c710184aaa802c209dbb235b213d

                    var products = Repository<Products>.GetEntityListForQuery(x => x.IsActive && x.CategoryId == Id, null, includes).Item1;
                    var userSelectedProducts = Repository<AspNetUserPreferences>.
                        GetEntityListForQuery(x => x.UserId == LoggedInUserId).Item1.Select(x => x.ProductId);

                    Result = JObject.FromObject(new
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
                               IsSelected = userSelectedProducts.Contains(product.id)
                           }
                        })
                    });
                    return GetOkResult(Result);
                }
                catch (Exception)
                {
                    Result = JObject.FromObject(new
                    {
<<<<<<< HEAD
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        ProductResult = ""
                    });
                    return GetOkResult(Result);
                }
=======
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
>>>>>>> 22fdf8d34de0c710184aaa802c209dbb235b213d
            }
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    ProductResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpPost]
        [Route("user/addtocart")]
        public async Task<IHttpActionResult> AddToCart(AddToCartModel addToCartModel)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    var newCartItem = new Cart
                    {
                        UserId = LoggedInUserId,
                        OfferId = addToCartModel.OfferId,
                        Quantity = addToCartModel.Quantity,
                        ProductId = addToCartModel.ProductId,
                        CategoryId = addToCartModel.CategoryId
                    };

                    await Repository<Cart>.InsertEntity(newCartItem, entity => { return entity.id; });

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "Item added to cart !",
                        AddToCartResult = ""
                    });

                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        AddToCartResult = ""
                    });
                    return GetOkResult(Result);
                }
            }
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    AddToCartResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("user/cart")]
        public async Task<IHttpActionResult> GetCart()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {

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
                                    CartId = product.id,
                                    Id = product.Products?.id,
                                    Name = product.Products?.Name,
                                    Category = product.Categories?.Name,
                                    Brand = product.Products?.Brand,
                                    Description = product.Products?.Description,
                                    Type = product.Products?.Type,
                                    Price = product.Products?.Price,
                                    OfferPrice = product.Products?.OfferPrice,
                                    product.Products?.MOQ,
                                    product.Products?.Quantity,
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
                        GetCartResult = ""
                    });
                    return GetOkResult(Result);
                }
            }
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    GetCartResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpPost]
        [Route("user/cart/update")]
        public async Task<IHttpActionResult> UpdateCart(UpdateCartModel addToCartModel)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {

                    var CartItem = Repository<Cart>.GetEntityListForQuery(x => x.id == addToCartModel.CartId).Item1.FirstOrDefault();
                    CartItem.Quantity = addToCartModel.Quantity;
                    await Repository<Cart>.UpdateEntity(CartItem, entity => { return entity.id; });

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "Cart Updated !",
                        UpdateCartResult = ""
                    });

                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        UpdateCartResult = ""
                    });
                    return GetOkResult(Result);
                }
            }
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    UpdateCartResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpDelete]
        [Route("user/cart/delete/{Id:int}")]
        public async Task<IHttpActionResult> deleteCart(int Id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {

                    var CartItem = Repository<Cart>.GetEntityListForQuery(x => x.id == Id).Item1.FirstOrDefault();
                    await Repository<Cart>.DeleteEntity(CartItem, entity => { return entity.id; });

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "Item Removed !",
                        DeleteCartResult = ""
                    });

                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        DeleteCartResult = ""
                    });
                    return GetOkResult(Result);
                }
            }
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    DeleteCartResult = ""
                });
                return GetOkResult(Result);
            }
        }


        [HttpPost]
        [Route("user/placeorder")]
        public async Task<IHttpActionResult> PlaceOrder(OrderModel orderModel)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
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
                        OrderResult = ""
                    });
                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        OrderResult = ""
                    });
                    return GetOkResult(Result);
                }
            }
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    OrderResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("user/orders")]
        public async Task<IHttpActionResult> GetOrders()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {


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
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    OrderResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("user/orders/{orderId:int}/details")]
        public async Task<IHttpActionResult> GetOrderDetails(int orderId)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {

                    List<Expression<Func<OrderDetails, Object>>> includes = new List<Expression<Func<OrderDetails, object>>>();
                    Expression<Func<OrderDetails, object>> IncludeProducts = (orderdetails) => orderdetails.Products;
                    Expression<Func<OrderDetails, object>> IncludeCategories = (orderdetails) => orderdetails.Categories;
                    includes.Add(IncludeProducts);
                    includes.Add(IncludeCategories);

                    var userOrders = Repository<OrderDetails>.
                        GetEntityListForQuery(x => x.OrderId == orderId, null, includes).Item1;

<<<<<<< HEAD
=======
                Result = JObject.FromObject(new
                {
                    status = true,
                    message = "",
>>>>>>> 22fdf8d34de0c710184aaa802c209dbb235b213d
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
            else
            {
                Result = JObject.FromObject(new
                {
                    status = false,
                    message = "Unauthorized",
                    OrderResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IHttpActionResult> LoginUser(LoginViewModel model)
        {
            AuthenticationResponse authenticationResponse = new AuthenticationResponse();
            
            var manager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signinManager = Request.GetOwinContext().GetUserManager<ApplicationSignInManager>();
            var user = manager.FindByName(model.Email);
            
            var validCredentials = signinManager.UserManager.CheckPassword(user, model.Password);
            if (validCredentials)
            {
                List<Expression<Func<AspNetUsers, Object>>> includesforProduct = new List<Expression<Func<AspNetUsers, object>>>();
                Expression<Func<AspNetUsers, object>> IncludeProduct = (product) => product.AspNetUserPreferences;
                includesforProduct.Add(IncludeProduct);

                var userDetails = Repository<AspNetUsers>.GetEntityListForQuery(x => x.Id == user.Id, null, includesforProduct).Item1.FirstOrDefault();

                authenticationResponse.UserId = userDetails.Id;
                authenticationResponse.EmailAddress = userDetails.Email;
                authenticationResponse.Name = userDetails.Name;
                authenticationResponse.FirstTimeLogin = userDetails.AspNetUserPreferences.Count() > 0 ? false : true;

            }

            JObject Result = null;
            try
            {

                Result = JObject.FromObject(new
                {
                    status = true,
                    message = authenticationResponse.UserId != null ? "Authentication Successfull !" : "Invalid Login Attempt !",
                    LoginResult = authenticationResponse
                });
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

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
    }
}
