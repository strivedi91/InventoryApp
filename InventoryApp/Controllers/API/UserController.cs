using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using InventoryApp.Helpers;
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
                try
                {
                    List<Expression<Func<Categories, Object>>> includesforProduct = new List<Expression<Func<Categories, object>>>();
                    Expression<Func<Categories, object>> IncludeProduct = (product) => product.Products;
                    includesforProduct.Add(IncludeProduct);
                    var categories = Repository<Categories>.GetEntityListForQuery(x => x.IsActive == true && x.IsDeleted == false, null, includesforProduct).Item1;
                    Result = JObject.FromObject(new
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
                    int category = saveUserPreferencesModel.SavePreferences.FirstOrDefault().CategoryId;
                    var existingPref = Repository<AspNetUserPreferences>.GetEntityListForQuery(x => x.UserId == LoggedInUserId && x.CategoryId == category).Item1;
                    if (existingPref != null)
                    {
                        Repository<AspNetUserPreferences>.DeleteRange(existingPref);
                    }

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
                        SavePreferencesResult = ""
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
                                       SelectedProductCount = Repository<AspNetUserPreferences>.GetEntityListForQuery(x => x.CategoryId == category.CategoryId && x.UserId == LoggedInUserId).Item1.Count()
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
                                    Item1.Select(x => new { x.QtyTo, x.QtyFrom, x.Price }),
                                    IsInCart = Repository<Cart>.GetEntityListForQuery(x => x.ProductId == product.Products.id && x.UserId == LoggedInUserId).Item1.Count() > 0 ? true : false,
                                    Images = GetProductImagesById(product.Products.id)
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

        private string[] GetProductImagesById(int productId)
        {

            string ProductImagePath = ConfigurationManager.AppSettings["ProductImagePath"].ToString();

            string path = Path.Combine((System.Web.Hosting.HostingEnvironment.ApplicationHost.ToString() + ProductImagePath), productId.ToString());

            string Productpath = Path.Combine(HttpContext.Current.Server.MapPath(ProductImagePath), productId.ToString());
            
            if (Directory.Exists(Productpath))
            {
                DirectoryInfo info = new DirectoryInfo(Productpath);
                FileInfo[] files = info.GetFiles("*.*");

                List<string> ProductImages = new List<string>();
                
                foreach (FileInfo file in files)
                {
                    string fileName = file.Name;
                    ProductImages.Add(Url.Content(ProductImagePath) + productId.ToString() + "/" + fileName);
                }
                return ProductImages.ToArray();
            }
            
            //if (Directory.Exists(path))
            //{
            //    return Directory.GetFiles(path);
            //}
            else
            {
                return new string[] { };
            }

        }

        [HttpGet]
        [Route("category/{Id:int}/products")]
        public async Task<IHttpActionResult> GetProductsByCategoryId(int Id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {

                    List<Expression<Func<Products, Object>>> includes = new List<Expression<Func<Products, object>>>();
                    Expression<Func<Products, object>> IncludeCategories = (category) => category.Categories;
                    Expression<Func<Products, object>> IncludeTierPricing = (pricing) => pricing.TierPricings;
                    Expression<Func<Products, object>> IncludeCart = (pricing) => pricing.Carts;
                    includes.Add(IncludeCategories);
                    includes.Add(IncludeTierPricing);
                    includes.Add(IncludeCart);

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
                               IsSelected = userSelectedProducts.Contains(product.id),
                               IsInCart = product.Carts.Where(x => x.UserId == LoggedInUserId).Count() > 0 ? true : false,
                               Images = GetProductImagesById(product.id)
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
                                    Item1.Select(x => new { x.QtyTo, x.QtyFrom, x.Price }),
                                    Images = GetProductImagesById(product.Products.id)
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

        [HttpGet]
        [Route("user/checkout")]
        public async Task<IHttpActionResult> Checkout()
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
                        CheckoutResult = JObject.FromObject(new
                        {
                            TotalAmount = userSelectedProducts.Sum(x => x.Quantity * x.Products.Price),
                            ShippingAddress = Repository<AspNetUsers>.GetEntityListForQuery(x => x.Id == LoggedInUserId).Item1.First()?.Address
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
                        CheckoutResult = ""
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
                    CheckoutResult = ""
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

                    List<Expression<Func<Cart, Object>>> includes = new List<Expression<Func<Cart, object>>>();
                    Expression<Func<Cart, object>> IncludeProducts = (product) => product.Products;
                    Expression<Func<Cart, object>> IncludeCategory = (category) => category.Categories;
                    includes.Add(IncludeProducts);
                    includes.Add(IncludeCategory);

                    var userSelectedProducts = Repository<Cart>.
                        GetEntityListForQuery(x => x.UserId == LoggedInUserId, null, includes).Item1;


                    Orders orders = new Orders
                    {
                        CreatedOn = DateTime.Now,
                        Discount = userSelectedProducts.Sum(x => x.Quantity * x.Products.Price) - userSelectedProducts.Sum(x => x.Quantity * Convert.ToDecimal(x.Products?.OfferPrice)),
                        OrderStatus = Enums.GetEnumDescription((Enums.OrderStatus.OrderPlaced)),
                        SubTotal = userSelectedProducts.Sum(x => x.Quantity * x.Products.Price),
                        Total = userSelectedProducts.Sum(x => x.Quantity * Convert.ToDecimal(x.Products?.OfferPrice)),
                        UserId = LoggedInUserId,
                        ShippingAddress = orderModel.ShippingAddress
                    };

                    await Repository<Orders>.InsertEntity(orders, entity => { return entity.id; });

                    List<OrderDetails> orderItems = new List<OrderDetails>();
                    orderItems.AddRange(
                        userSelectedProducts.
                        Select(x => new OrderDetails
                        {
                            OrderId = orders.id,
                            Quantity = x.Quantity,
                            CategoryId = x.CategoryId,
                            Discount = 0,
                            Price = x.Products.Price,
                            TotalPrice = x.Products.Price,
                            ProductId = x.ProductId
                        }));

                    await Repository<OrderDetails>.InsertMultipleEntities(orderItems);

                    await Repository<Cart>.DeleteRange(userSelectedProducts);

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "Order placed successfully !",
                        PlaceOrderResult = ""
                    });
                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        PlaceOrderResult = ""
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
                        OrderResult = JObject.FromObject(new
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
                                    order.Total,
                                    order.ShippingAddress,
                                    Images = getProductImages(order.id)
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

        public string[] getProductImages(int OrderId)
        {
            var orderDetail = Repository<OrderDetails>.GetEntityListForQuery(x => x.OrderId == OrderId).Item1;
            List<string> orderedItemImages = new List<string>();
            foreach(var orderItem in orderDetail)
            {
                string[] stProdImaged = GetProductImagesById(orderItem.ProductId);
                if(stProdImaged.Count() > 0)
                {
                    foreach(string image in stProdImaged)
                    {
                        orderedItemImages.Add(image);
                    }                    
                }
            }
            return orderedItemImages.ToArray();
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

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "",
                        OrderDetailsResult = JObject.FromObject(new
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
                                OrderedQuantity = product.Quantity,
                                Images = GetProductImagesById(product.id)
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
                        OrderDetailsResult = ""
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
                    OrderDetailsResult = ""
                });
                return GetOkResult(Result);
            }
        }


        [HttpGet]
        [Route("user/orders/{orderId:int}/cancel")]
        public async Task<IHttpActionResult> CancelOrder(int orderId)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    var userOrders = Repository<Orders>.
                        GetEntityListForQuery(x => x.id == orderId, null, null).Item1.FirstOrDefault();

                    userOrders.OrderStatus = Enums.GetEnumDescription((Enums.OrderStatus.Cancelled));

                    await Repository<Orders>.UpdateEntity(userOrders, entity => { return entity.id; });

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "Order Cancelled Successfully !",
                        CancelOrderResult = ""
                    });
                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        CancelOrderResult = ""
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
                    CancelOrderResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpPost]
        [Route("user/changepassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    var manager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var signinManager = Request.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                    var user = manager.FindById(LoggedInUserId);

                    var result = await manager.ChangePasswordAsync(LoggedInUserId, changePasswordViewModel.OldPassword, changePasswordViewModel.NewPassword);
                    Result = JObject.FromObject(new
                    {
                        status = result.Succeeded ? true : false,
                        message = result.Errors.Count() == 0 ? "Password Changed Successfully!" : result.Errors.FirstOrDefault(),
                        ChangePasswordResult = ""
                    });
                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        ChangePasswordResult = ""
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
                    ChangePasswordResult = ""
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
                authenticationResponse.Address = userDetails.Address;
            }

            JObject Result = null;
            try
            {

                Result = JObject.FromObject(new
                {
                    status = authenticationResponse.UserId != null ? true : false,
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