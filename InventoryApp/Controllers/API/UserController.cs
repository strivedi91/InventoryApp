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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InventoryApp.Controllers.API
{
    [RoutePrefix("api")]
    public class UserController : BaseAPIController
    {
        #region public methods
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
                                    let Offer = Repository<Offers>.GetEntityListForQuery(x => x.CategoryId == category.Id).Item1.FirstOrDefault()
                                    select new
                                    {
                                        Id = category.Id,
                                        Name = category.Name,
                                        Offer = Offer ?? new Offers(),
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
                                   let Offer = Repository<Offers>.GetEntityListForQuery(x => x.CategoryId == category.Categories.Id).Item1.FirstOrDefault()
                                   select new
                                   {
                                       Id = category.Categories.Id,
                                       Name = category.Categories.Name,
                                       Offer = Offer ?? new Offers(),
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
                                        let Offer = Repository<Offers>.GetEntityListForQuery(x => x.CategoryId == category.Id).Item1.FirstOrDefault()
                                        select new
                                        {

                                            Id = category.Id,
                                            Name = category.Name,
                                            Offer = Offer ?? new Offers(),
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
                    Expression<Func<AspNetUserPreferences, object>> IncludeOffer = (Offer) => Offer.Products.Offers;
                    includes.Add(IncludeCategories);
                    includes.Add(IncludeProducts);
                    includes.Add(IncludeOffer);

                    var userSelectedProducts = Repository<AspNetUserPreferences>.
                        GetEntityListForQuery(x => x.UserId == LoggedInUserId && x.CategoryId == Id && x.Products.IsActive == true, null, includes).Item1;

                    var cart = Repository<Cart>.GetEntityListForQuery(x => x.UserId == LoggedInUserId).Item1;

                    var categoryOffer = Repository<Offers>.GetEntityListForQuery(x => x.CategoryId == Id && x.IsDeleted == false && x.IsActive == true).Item1;

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
                                    CategoryId = product.CategoryId,
                                    Brand = product.Products.Brand,
                                    Description = product.Products.Description,
                                    Type = product.Products.Type,
                                    Price = product.Products.Price + (product.Products.ApplyGst ? (product.Products.Price * product.Categories.GST) / 100 : 0),
                                    OfferPrice = product.Products.OfferPrice == null ? 0 : product.Products.OfferPrice,
                                    MOQ = product.Products.MOQ == null ? 1 : product.Products.MOQ,
                                    GST = product.Products.ApplyGst ? product.Categories.GST : 0,
                                    product.Products.Quantity,
                                    OfferDetails = mergeOffers(categoryOffer, product.Products.Offers.Where(x => x.IsDeleted == false && x.IsActive == true))
                                              .Select(x => new
                                              {
                                                  x.id,
                                                  x.OfferCode,
                                                  x.OfferDescription,
                                                  x.FlatDiscount,
                                                  x.PercentageDiscount,
                                                  x.CategoryId,
                                                  x.ProductId,
                                                  x.StartDate,
                                                  x.EndDate,
                                                  IsSelected = Repository<Cart>.GetEntityListForQuery(c => c.OfferId == x.id && x.IsDeleted == false && c.AspNetUsers.Id == LoggedInUserId).Item2 > 0 ? true : false,
                                                  //IsUsed = Repository<OrderDetails>.GetEntityListForQuery(o => o.OfferId == x.id && x.IsDeleted == false && o.Orders.UserId == LoggedInUserId).Item2 > 0 ? true : false,
                                              }),
                                    TierPricing = Repository<TierPricing>.GetEntityListForQuery(x => x.ProductId == product.Products.id && x.IsActive && x.IsDeleted == false).
                                    Item1.Select(x => new { x.QtyTo, x.QtyFrom, x.Price }),
                                    IsInCart = cart.Where(x => x.ProductId == product.Products.id).Count() > 0 ? true : false,
                                    SelectedQuantity = cart.Where(x => x.ProductId == product.ProductId).Select(x => x.Quantity).FirstOrDefault(),
                                    //OfferId = cart.Where(x => x.ProductId == product.Products.id).Select(x => x.OfferId),
                                    IsInWishList = Repository<WishList>.GetEntityListForQuery(x => x.ProductId == product.Products.id && x.UserId == LoggedInUserId).Item1.Count() > 0 ? true : false,
                                    //Reviews = Repository<ProductReview>.GetEntityListForQuery(x => x.ProductId == product.ProductId),
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

        [HttpGet]
        [Route("product/{Id:int}/review")]
        public async Task<IHttpActionResult> GetProductReviewById(int Id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    List<Expression<Func<ProductReview, Object>>> includes = new List<Expression<Func<ProductReview, object>>>();
                    Expression<Func<ProductReview, object>> IncludeUser = (user) => user.AspNetUsers;
                    includes.Add(IncludeUser);

                    var productReviews = Repository<ProductReview>.GetEntityListForQuery(x => x.ProductId == Id, null, includes).Item1;
                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "",
                        ProductReviewResult = JObject.FromObject(new
                        {
                            Reviews =
                           from review in productReviews
                           select new
                           {
                               review.Id,
                               review.Rating,
                               review.Review,
                               review.CreatedOn,
                               review.AspNetUsers.UserName
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
                    Expression<Func<Products, object>> IncludeOffer = (Offer) => Offer.Offers;
                    //Expression<Func<Products, object>> IncludeReview = (review) => review.ProductReviews;
                    includes.Add(IncludeCategories);
                    includes.Add(IncludeTierPricing);
                    includes.Add(IncludeCart);
                    includes.Add(IncludeOffer);
                    //includes.Add(IncludeReview);

                    var products = Repository<Products>.GetEntityListForQuery(x => x.IsActive && x.CategoryId == Id, null, includes).Item1;
                    var userSelectedProducts = Repository<AspNetUserPreferences>.
                        GetEntityListForQuery(x => x.UserId == LoggedInUserId).Item1.Select(x => x.ProductId);


                    var categoryOffer = Repository<Offers>.GetEntityListForQuery(x => x.CategoryId == Id && x.IsDeleted == false && x.IsActive == true).Item1;

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
                               CategoryId = product.CategoryId,
                               Brand = product.Brand,
                               Description = product.Description,
                               Type = product.Type,
                               Price = product.Price + (product.ApplyGst ? (product.Price * product.Categories.GST) / 100 : 0),
                               GST = product.ApplyGst ? product.Categories.GST : 0,
                               OfferPrice = product.OfferPrice == null ? 0 : product.OfferPrice,
                               MOQ = product.MOQ == null ? 1 : product.MOQ,
                               product.Quantity,
                               OfferDetails = mergeOffers(categoryOffer, product.Offers.Where(x => x.IsDeleted == false && x.IsActive == true))
                                              .Select(x => new
                                              {
                                                  x.id,
                                                  x.OfferCode,
                                                  x.OfferDescription,
                                                  x.FlatDiscount,
                                                  x.PercentageDiscount,
                                                  x.CategoryId,
                                                  x.ProductId,
                                                  x.StartDate,
                                                  x.EndDate,
                                                  IsSelected = Repository<Cart>.GetEntityListForQuery(c => c.OfferId == x.id && x.IsDeleted == false && c.AspNetUsers.Id == LoggedInUserId).Item2 > 0 ? true : false,
                                                  //IsUsed = Repository<OrderDetails>.GetEntityListForQuery(o => o.OfferId == x.id && x.IsDeleted == false && o.Orders.UserId == LoggedInUserId).Item2 > 0 ? true : false,
                                              }),
                               TierPricing = product.TierPricings.Select(x => new { x.QtyTo, x.QtyFrom, x.Price }),
                               IsSelected = userSelectedProducts.Contains(product.id),
                               IsInCart = product.Carts.Where(x => x.UserId == LoggedInUserId).Count() > 0 ? true : false,
                               OfferId = product.Carts.Where(x => x.ProductId == product.id).Select(x => x.OfferId),
                               SelectedQuantity = product.Carts.Where(x => x.ProductId == product.id).Select(x => x.Quantity).FirstOrDefault(),
                               IsInWishList = product.WishLists.Where(x => x.UserId == LoggedInUserId).Count() > 0 ? true : false,
                               //Reviews=product.ProductReviews,
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
                    var product = Repository<Products>.GetEntityListForQuery(x => x.id == addToCartModel.ProductId).Item1.FirstOrDefault();

                    var newCartItem = new Cart
                    {
                        UserId = LoggedInUserId,
                        Quantity = addToCartModel.Quantity == 0 ? Convert.ToInt32(product.MOQ) : addToCartModel.Quantity,
                        ProductId = addToCartModel.ProductId,
                        CategoryId = addToCartModel.CategoryId == 0 ? product.CategoryId : addToCartModel.CategoryId
                    };

                    if (addToCartModel.OfferId != 0)
                    {
                        newCartItem.OfferId = addToCartModel.OfferId;
                    }
                    await Repository<Cart>.InsertEntity(newCartItem, entity => { return entity.id; });

                    if (newCartItem.id > 0)
                    {
                        var WishListItem = Repository<WishList>.GetEntityListForQuery(x => x.UserId == LoggedInUserId && x.ProductId == addToCartModel.ProductId).Item1.FirstOrDefault();
                        if (WishListItem != null)
                        {
                            await Repository<WishList>.DeleteEntity(WishListItem, entity => { return entity.Id; });
                        }
                    }
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
                    Expression<Func<Cart, object>> IncludeOffers = (offers) => offers.Offers;
                    includes.Add(IncludeProducts);
                    includes.Add(IncludeCategory);
                    includes.Add(IncludeOffers);

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
                                    CategoryId = product.Products?.CategoryId,
                                    Brand = product.Products?.Brand,
                                    Description = product.Products?.Description,
                                    Type = product.Products?.Type,
                                    Price = product.Products?.Price,
                                    OfferPrice = product.Products.OfferPrice == null ? 0 : product.Products.OfferPrice,
                                    MOQ = product.Products.MOQ == null ? 1 : product.Products.MOQ,
                                    product.Products?.Quantity,
                                    SelectedQuantity = product.Quantity,
                                    OfferDetails = Repository<Offers>.GetEntityListForQuery(x => x.IsDeleted == false && x.IsActive == true).Item1.
                                    Select(x => new
                                    {
                                        x.id,
                                        x.OfferCode,
                                        x.OfferDescription,
                                        x.FlatDiscount,
                                        x.PercentageDiscount,
                                        x.StartDate,
                                        x.EndDate,
                                        IsSelected = (x.id == product.OfferId)
                                    }),
                                    //OfferDetails = Repository<Offers>.GetEntityListForQuery(x => x.id == product.OfferId && x.IsDeleted == false).
                                    //Item1.Select(x => new { x.OfferCode, x.OfferDescription, x.FlatDiscount, x.PercentageDiscount, x.StartDate, x.EndDate }),
                                    TierPricing = Repository<TierPricing>.GetEntityListForQuery(x => x.ProductId == product.Products.id && x.IsActive && x.IsDeleted == false).
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

                    var TotalAmount = userSelectedProducts.Sum(x => x.Products.OfferPrice == null ? x.Quantity * x.Products.Price : x.Quantity * x.Products.OfferPrice);

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "",

                        CheckoutResult = JObject.FromObject(new
                        {
                            TotalAmount = TotalAmount, //userSelectedProducts.Sum(x => x.Quantity * x.Products.OfferPrice),
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
                    if (addToCartModel.OfferId != 0)
                    {
                        CartItem.OfferId = addToCartModel.OfferId;
                    }
                    else
                    {
                        CartItem.OfferId = null;
                    }

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
                    Expression<Func<Cart, object>> IncludeOffers = (offer) => offer.Offers;
                    includes.Add(IncludeProducts);
                    includes.Add(IncludeCategory);
                    includes.Add(IncludeOffers);

                    var userSelectedProducts = Repository<Cart>.
                        GetEntityListForQuery(x => x.UserId == LoggedInUserId, null, includes).Item1;


                    decimal flatDiscount = userSelectedProducts.Where(x => x.Offers != null && x.Offers?.FlatDiscount > 0).Sum(x => Convert.ToDecimal(x.Offers?.FlatDiscount));

                    decimal percentageDiscont = userSelectedProducts.Where(x => x.Offers != null && x.Offers?.PercentageDiscount > 0).Sum(x => (Convert.ToDecimal(x.Offers?.PercentageDiscount * (x.Products.OfferPrice != null ? x.Products?.OfferPrice : x.Products.Price)) / 100));

                    decimal CouponDiscount = flatDiscount + percentageDiscont;

                    Orders orders = new Orders
                    {
                        CreatedOn = DateTime.Now,
                        Discount = (userSelectedProducts.Sum(x => x.Quantity * x.Products.Price) - userSelectedProducts.Sum(x => x.Quantity * Convert.ToDecimal(x.Products.OfferPrice != null ? x.Products?.OfferPrice : x.Products.Price))) + CouponDiscount,
                        OrderStatus = Enums.GetEnumDescription((Enums.OrderStatus.OrderPlaced)),
                        SubTotal = userSelectedProducts.Sum(x => x.Quantity * x.Products.Price),
                        Total = userSelectedProducts.Sum(x => x.Quantity * Convert.ToDecimal(x.Products.OfferPrice != null ? x.Products?.OfferPrice : x.Products.Price)) - CouponDiscount,
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
                            Discount = Convert.ToDecimal((x.Quantity * x.Products.Price) - Convert.ToDecimal(x.Quantity * (x.Products.OfferPrice != null ? x.Products?.OfferPrice : x.Products.Price))) + Convert.ToDecimal(x.Offers?.PercentageDiscount > 0 ? ((x.Offers?.PercentageDiscount * (x.Products.OfferPrice != null ? x.Products?.OfferPrice : x.Products.Price)) / 100) : x.Offers?.FlatDiscount),
                            Price = x.Products.Price,
                            TotalPrice = (Convert.ToDecimal(x.Quantity * (x.Products.OfferPrice != null ? x.Products?.OfferPrice : x.Products.Price)) - Convert.ToDecimal(x.Offers?.PercentageDiscount > 0 ? ((x.Offers?.PercentageDiscount * (x.Products.OfferPrice != null ? x.Products?.OfferPrice : x.Products.Price)) / 100) : x.Offers?.FlatDiscount)),
                            ProductId = x.ProductId,
                            OfferId = x.OfferId == 0 ? null : x.OfferId,
                            OfferCode = x.Offers?.OfferCode,
                            OfferDescription = x.Offers?.OfferDescription,
                            FlatDiscount = x.Offers?.FlatDiscount,
                            PercentageDiscount = x.Offers?.PercentageDiscount,
                        }));

                    await Repository<OrderDetails>.InsertMultipleEntities(orderItems);

                    await Repository<Cart>.DeleteRange(userSelectedProducts);

                    await AdjustQuantity(orderItems);

                    #region PDF
                    string lsPDFBody = string.Empty;

                    string fsFilePath = ConfigurationManager.AppSettings["OrderInvoicePdfPath"];
                    string fsFileName = "Order_Invoice_" + orders.id + ".pdf";

                    using (StreamReader loStreamReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["OrderInvoiceTemplatePath"])))
                    {
                        string lsLine = string.Empty;
                        while ((lsLine = loStreamReader.ReadLine()) != null)
                        {
                            lsPDFBody += lsLine;
                        }

                        string itemList = "<tr>"
                                           + "<td></td>"
                                           + "<td></td>"
                                           + "<td></td>"
                                           + "<td></td>"
                                           + "<td></td>"
                                           + "<td></td>"
                                           + "</tr>";
                        var counter = 1;
                        foreach (var item in orderItems)
                        {
                            itemList += "<tr>"
                                        + "<td>" + counter++ + "</td>"
                                        + "<td>" + Repository<Products>.GetEntityListForQuery(x => x.id == item.ProductId).Item1.Select(y => y.Name).FirstOrDefault() + "</td>"
                                        //+ "<td>" + item.OfferDescription + "</td>"
                                        //+ "<td>" + item.CategoryId + "</td>"
                                        //+ "<td>" + item.Products.Type + "</td>"
                                        //+ "<td>" + item.Products.Brand + "</td>"
                                        + "<td>" + item.Price + "</td>"
                                        + "<td>" + item.Quantity + "</td>"
                                        + "<td>" + item.Discount + "</td>"
                                        + "<td>" + item.TotalPrice + "</td>"
                                        + "</tr>";
                        }

                        lsPDFBody = lsPDFBody.Replace("{SellerName}", Repository<AspNetUsers>.GetEntityListForQuery(x => x.Id == orders.UserId).Item1.Select(y => y.Name).FirstOrDefault());
                        lsPDFBody = lsPDFBody.Replace("{OrderDate}", orders.CreatedOn.ToShortDateString());
                        lsPDFBody = lsPDFBody.Replace("{OrderAddress}", orders.ShippingAddress);
                        lsPDFBody = lsPDFBody.Replace("{ProductList}", itemList);
                        lsPDFBody = lsPDFBody.Replace("{SubTotal}", orders.SubTotal.ToString());
                        lsPDFBody = lsPDFBody.Replace("{Discount}", orders.Discount.ToString());
                        lsPDFBody = lsPDFBody.Replace("{Total}", orders.Total.ToString());


                        StringReader loStringReader = new StringReader(lsPDFBody);

                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(HttpContext.Current.Server.MapPath(fsFilePath + fsFileName), FileMode.Create));

                        pdfDoc.Open();

                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, loStringReader);

                        pdfDoc.Close();
                    }
                    #endregion

                    bool sendEmailResult = CommonFunctions.sendPlacedOrderEmail(orders, orderItems, "Placed");

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
                                    ShippingAddress = string.IsNullOrEmpty(order.ShippingAddress) ? "" : order.ShippingAddress,
                                    OrderInvoice = getOrderInvoiceUrl(order.id),
                                    Images = getProductImages(order.id),
                                    CancellationReason = string.IsNullOrEmpty(order.CancellationReason) ? "" : order.CancellationReason
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

        [HttpGet]
        [Route("user/offers/{Id:int}")]
        public async Task<IHttpActionResult> GetOffers(int Id)
        {
            JObject Result = null;

            try
            {
                List<Expression<Func<Offers, Object>>> includes = new List<Expression<Func<Offers, object>>>();
                Expression<Func<Offers, object>> IncludeProducts = (products) => products.Products;
                Expression<Func<Offers, object>> IncludeCategorys = (categories) => categories.Categories;
                includes.Add(IncludeProducts);
                includes.Add(IncludeCategorys);

                var offers = Repository<Offers>.
                    GetEntityListForQuery(x => x.id == Id, null, includes).Item1;

                Result = JObject.FromObject(new
                {
                    status = true,
                    message = "",
                    OfferResult = from offer in offers
                                  select new
                                  {
                                      Id = offer.id,
                                      OfferCode = offer.OfferCode,
                                      OfferDescription = offer.OfferDescription,
                                      FlatDiscount = offer.FlatDiscount,
                                      PercentageDiscount = offer.PercentageDiscount,
                                      Category = offer.Categories != null ? offer.Categories.Name : "",
                                      Product = offer.Products != null ? offer.Products.Name : "",
                                      StartDate = offer.StartDate,
                                      EndDate = offer.EndDate,
                                      IsActive = offer.IsActive
                                  }
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

        public string[] getProductImages(int OrderId)
        {
            var orderDetail = Repository<OrderDetails>.GetEntityListForQuery(x => x.OrderId == OrderId).Item1;
            List<string> orderedItemImages = new List<string>();
            foreach (var orderItem in orderDetail)
            {
                string[] stProdImaged = GetProductImagesById(orderItem.ProductId);
                if (stProdImaged.Count() > 0)
                {
                    foreach (string image in stProdImaged)
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
                                OfferPrice = product.Products.OfferPrice == null ? 0 : product.Products.OfferPrice,
                                MOQ = product.Products.MOQ == null ? 1 : product.Products.MOQ,
                                product.Products.Quantity,
                                OrderedQuantity = product.Quantity,
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


        [HttpPost]
        [Route("user/orders/cancel")]
        public async Task<IHttpActionResult> CancelOrder(CancelOrderModel cancelOrderModel)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    var userOrders = Repository<Orders>.
                        GetEntityListForQuery(x => x.id == cancelOrderModel.OrderId, null, null).Item1.FirstOrDefault();

                    userOrders.OrderStatus = Enums.GetEnumDescription((Enums.OrderStatus.Cancelled));
                    userOrders.CancellationReason = cancelOrderModel.CancelReason;

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
                authenticationResponse.CartCount = Repository<Cart>.GetEntityListForQuery(x => x.UserId == userDetails.Id).Item2;
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

        [HttpPost]
        [Route("user/addremovewishlist")]
        public async Task<IHttpActionResult> addRemoveWishList(AddToWishListModel foRequest)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                foRequest.UserId = LoggedInUserId;
                try
                {
                    if (foRequest.Operation.ToLower() == "add")
                    {
                        var prodcut = Repository<Products>.GetEntityListForQuery(x => x.id == foRequest.ProductId).Item1.FirstOrDefault();

                        var newWishList = new WishList
                        {
                            CategoryId = prodcut.CategoryId,
                            ProductId = foRequest.ProductId,
                            UserId = LoggedInUserId
                        };

                        await Repository<WishList>.InsertEntity(newWishList, entity => { return entity.Id; });

                        if (newWishList.Id > 0)
                        {
                            var CartItem = Repository<Cart>.GetEntityListForQuery(x => x.ProductId == foRequest.ProductId && x.UserId == LoggedInUserId).Item1.FirstOrDefault();
                            if (CartItem != null)
                                await Repository<Cart>.DeleteEntity(CartItem, entity => { return entity.id; });
                        }
                        Result = JObject.FromObject(new
                        {
                            status = true,
                            message = "Item added to wish list !",
                            WishListId = newWishList.Id,
                            AddWishListResult = ""
                        });
                    }
                    else if (foRequest.Operation.ToLower() == "remove")
                    {
                        var WishListItem = Repository<WishList>.GetEntityListForQuery(x => x.UserId == LoggedInUserId && x.ProductId == foRequest.ProductId).Item1.FirstOrDefault();
                        if (WishListItem != null)
                        {
                            await Repository<WishList>.DeleteEntity(WishListItem, entity => { return entity.Id; });

                            Result = JObject.FromObject(new
                            {
                                status = true,
                                message = "Item Removed !",
                                WishListId = WishListItem.Id,
                                DeleteWishListResult = ""
                            });
                        }
                    }

                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        AddWishListResult = ""
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
                    AddWishListResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("notifications")]
        public async Task<IHttpActionResult> getNotificaions()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    var notifications = Repository<NotificationHistory>.
                        GetEntityListForQuery(null).Item1;

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "",
                        NotificationResult = JObject.FromObject(new
                        {

                            Products =
                                from notification in notifications
                                select new
                                {
                                    notification.Id,
                                    notification.NotificationText,
                                    notification.CreatedOn
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
                        NotificationResult = ""
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
                    NotificationResult = ""
                });
                return GetOkResult(Result);
            }
        }


        [HttpGet]
        [Route("user/wishlist")]
        public async Task<IHttpActionResult> getWishList()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    List<Expression<Func<WishList, Object>>> includes = new List<Expression<Func<WishList, object>>>();
                    Expression<Func<WishList, object>> IncludeProducts = (product) => product.Products;
                    Expression<Func<WishList, object>> IncludeCategory = (category) => category.Categories;
                    includes.Add(IncludeProducts);
                    includes.Add(IncludeCategory);

                    var userSelectedProducts = Repository<WishList>.
                        GetEntityListForQuery(x => x.UserId == LoggedInUserId, null, includes).Item1;

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "",
                        WishListResult = JObject.FromObject(new
                        {

                            Products =
                                from product in userSelectedProducts
                                select new
                                {
                                    WishListId = product.Id,
                                    ProductId = product.Products?.id,
                                    ProductName = product.Products?.Name,
                                    Category = product.Categories?.Name,
                                    CategoryId = product.Products?.CategoryId,
                                    Brand = product.Products?.Brand,
                                    Description = product.Products?.Description,
                                    Type = product.Products?.Type,
                                    Price = product.Products?.Price,
                                    OfferPrice = product.Products.OfferPrice == null ? 0 : product.Products.OfferPrice,
                                    MOQ = product.Products.MOQ == null ? 1 : product.Products.MOQ,
                                    product.Products?.Quantity,
                                    TierPricing = Repository<TierPricing>.GetEntityListForQuery(x => x.ProductId == product.Products.id && x.IsActive && x.IsDeleted == false).
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
                        WishListResult = ""
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
                    WishListResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpDelete]
        [Route("user/wishlist/delete/{Id:int}")]
        public async Task<IHttpActionResult> deleteWishList(int Id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                try
                {
                    var WishListItem = Repository<WishList>.GetEntityListForQuery(x => x.Id == Id).Item1.FirstOrDefault();
                    await Repository<WishList>.DeleteEntity(WishListItem, entity => { return entity.Id; });

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "Item Removed !",
                        DeleteWishListResult = ""
                    });

                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        DeleteWishListResult = ""
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
                    DeleteWishListResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpPost]
        [Route("user/suggestion")]
        public async Task<IHttpActionResult> addSuggestion(SuggestionModel foRequest)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;
            if (LoggedInUserId != null)
            {
                foRequest.UserId = LoggedInUserId;
                try
                {
                    var newSuggestion = new Suggestions
                    {
                        Suggestion = foRequest.Suggestion,
                        ProductId = foRequest.ProductId,
                        UserId = LoggedInUserId
                    };

                    await Repository<Suggestions>.InsertEntity(newSuggestion, entity => { return entity.Id; });


                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "Suggestion added !",
                        SuggestionId = newSuggestion.Id,
                        AddSuggestionResult = ""
                    });

                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        AddWishListResult = ""
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
                    AddWishListResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpPost]
        [Route("user/feedback")]
        public async Task<IHttpActionResult> addProductReview(SaveProductReviewModel foRequest)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;

            if (LoggedInUserId != null)
            {
                try
                {
                    var newSuggestion = new ProductReview
                    {
                        Review = foRequest.Review,
                        ProductId = foRequest.ProductId,
                        Rating = foRequest.Rating,
                        UserId = LoggedInUserId
                    };

                    await Repository<ProductReview>.InsertEntity(newSuggestion, entity => { return entity.Id; });


                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "Review added !",
                        SuggestionId = newSuggestion.Id,
                        AddSuggestionResult = ""
                    });

                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        AddWishListResult = ""
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
                    AddWishListResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpPost]
        [Route("user/updatedeviceid")]
        public async Task<IHttpActionResult> updateDeviceId(UpdateDeviceIdModel foRequest)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;

            var user = Repository<AspNetUsers>.GetEntityListForQuery(x => x.Id == LoggedInUserId).Item1.FirstOrDefault();

            if (LoggedInUserId != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(foRequest.DeviceId))
                    {
                        user.DeviceId = foRequest.DeviceId;

                        await Repository<AspNetUsers>.UpdateUserEntity(user, (entity) => { return entity.Id; });
                    }

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "Device Id Updated!",
                        UpdateDeviceIdResult = ""
                    });

                    return GetOkResult(Result);
                }
                catch (Exception ex)
                {
                    Result = JObject.FromObject(new
                    {
                        status = false,
                        message = "Sorry, there was an error processing your request. Please try again !",
                        UpdateDeviceIdResult = ""
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
                    UpdateDeviceIdResult = ""
                });
                return GetOkResult(Result);
            }
        }

        [HttpGet]
        [Route("getadvertisements")]
        public async Task<IHttpActionResult> getAdvertisements()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("UserId");
            var LoggedInUserId = headerValues.FirstOrDefault();
            JObject Result = null;

            if (LoggedInUserId != null)
            {
                try
                {
                    string ProductImagePath = ConfigurationManager.AppSettings["AdvertisementImagePath"].ToString();

                    string path = Path.Combine((System.Web.Hosting.HostingEnvironment.ApplicationHost.ToString() + ProductImagePath));

                    string Productpath = Path.Combine(HttpContext.Current.Server.MapPath(ProductImagePath));

                    List<string> ProductImages = new List<string>();
                    if (Directory.Exists(Productpath))
                    {
                        DirectoryInfo info = new DirectoryInfo(Productpath);
                        FileInfo[] files = info.GetFiles("*.*");
                        foreach (FileInfo file in files)
                        {
                            string fileName = file.Name;
                            ProductImages.Add(Url.Content(ProductImagePath) + "/" + fileName);
                        }
                    }

                    Result = JObject.FromObject(new
                    {
                        status = true,
                        message = "",
                        AdvertisementResult = JObject.FromObject(new
                        {
                            Images = ProductImages.ToArray()
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
                        AdvertisementResult = ""
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
                    AdvertisementResult = ""
                });
                return GetOkResult(Result);
            }
        }

        #endregion

        #region private methodes
        private bool sendPlacedOrderEmail(Orders foOrder, List<OrderDetails> foOrderItems)
        {
            try
            {
                AspNetUsers loUser = Repository<AspNetUsers>.GetEntityListForQuery(x => x.Id == foOrder.UserId).Item1.FirstOrDefault();



                string lsFrom = "no-replay@thgodowninventorryapp.com";

                string lsToMails = loUser.Email;

                string lsSubject = string.Empty;

                string lsEmailBody = string.Empty;

                lsSubject = "Your Order Status";

                using (StreamReader loStreamReader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/Views/Shared/EmailTemplates/OrderStatus.html")))
                {
                    string lsLine = string.Empty;
                    while ((lsLine = loStreamReader.ReadLine()) != null)
                    {
                        lsEmailBody += lsLine;
                    }
                }

                string OrderItemRow = string.Empty;
                decimal TotalAmount = 0;
                foreach (var item in foOrderItems)
                {
                    Products loProducts = Repository<Products>.GetEntityListForQuery(x => x.id == item.ProductId).Item1.FirstOrDefault();

                    OrderItemRow += OrderItemRow + "<tr>"
                                    + "<td width='15%' style='border-bottom:1px solid #676161; padding:10px; text-align:left'><img src='" + GetProductImagesById(loProducts.id)[0] + "' style='height:50px;width:50px'></td>"
                                    + "<td width='30%' style='border-bottom:1px solid #676161; padding:10px; text-align:left'>" + loProducts.Name + "</td>"
                                    + "<td width='15%' style='border-bottom:1px solid #676161; padding:10px; text-align:center'>" + item.Quantity + "</td>"
                                    + "<td width='20%' style='border-bottom:1px solid #676161; padding:10px; text-align:righ'>" + item.Price + "</td>"
                                    + "<td width='20%' style='border-bottom:1px solid #676161; padding:10px; text-align:righ'>" + item.TotalPrice + "</td>"
                                    + "</tr>";

                    TotalAmount += TotalAmount + item.TotalPrice;
                }

                lsEmailBody = lsEmailBody.Replace("{OrderNo}", foOrder.id.ToString());
                lsEmailBody = lsEmailBody.Replace("{OrderDate}", foOrder.CreatedOn.ToShortDateString());
                lsEmailBody = lsEmailBody.Replace("{OrderStatus}", foOrder.OrderStatus);
                lsEmailBody = lsEmailBody.Replace("{CustomerName}", loUser.Name);
                lsEmailBody = lsEmailBody.Replace("{Address}", foOrder.ShippingAddress);
                lsEmailBody = lsEmailBody.Replace("{OrderItemRows}", OrderItemRow);
                lsEmailBody = lsEmailBody.Replace("{TotalAmount}", TotalAmount.ToString());

                return EmailHelper.sendEmail(lsToMails, lsFrom, lsSubject, lsEmailBody);
            }
            catch (Exception ex)
            {
                return false;
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

        private string getOrderInvoiceUrl(int OrderId)
        {
            string PDFPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["OrderInvoicePdfPath"]);

            string PdfName = "Order_Invoice_" + OrderId.ToString() + ".pdf";

            if (Directory.Exists(PDFPath))
            {
                DirectoryInfo info = new DirectoryInfo(PDFPath);
                FileInfo[] files = info.GetFiles(PdfName);

                if (files != null && files.Count() > 0 && files[0].Name.ToLower() == PdfName.ToLower())
                    return Url.Content(ConfigurationManager.AppSettings["OrderInvoicePdfPath"] + PdfName);
                else
                    return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        private List<Offers> mergeOffers(IEnumerable<Offers> foOffers1, IEnumerable<Offers> foOffers2)
        {
            List<Offers> loAllOffers = new List<Offers>();

            loAllOffers.AddRange(foOffers1);
            loAllOffers.AddRange(foOffers2);

            return loAllOffers;
        }

        private async Task AdjustQuantity(List<OrderDetails> orderDetails)
        {
            foreach (var product in orderDetails)
            {
                var p = Repository<Products>.GetEntityListForQuery(x => x.id == product.ProductId).Item1.FirstOrDefault();
                p.Quantity = p.Quantity - product.Quantity;
                await Repository<Products>.UpdateEntity(p, (entity) => { return entity.id; });
            }
        }
        #endregion
    }
}