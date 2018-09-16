using InventoryApp.Areas.Admin.Models;
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
    public class OrderDetailsController : Controller
    {
        // GET: Admin/OrderDetails
        public ActionResult Index(int OrderId)
        {
            List<Expression<Func<OrderDetails, Object>>> includes = new List<Expression<Func<OrderDetails, object>>>();
            Expression<Func<OrderDetails, object>> IncludeProducts = (orderdetails) => orderdetails.Products;
            Expression<Func<OrderDetails, object>> IncludeCategories = (orderdetails) => orderdetails.Categories;
            includes.Add(IncludeProducts);
            includes.Add(IncludeCategories);

            var userOrders = Repository<OrderDetails>.
                GetEntityListForQuery(x => x.OrderId == OrderId, null, includes).Item1;

            List<ProductModel> productModels = new List<ProductModel>();
            OrderDetailsModel orderDetails = new OrderDetailsModel();
            foreach (var product in userOrders)
            {
                ProductModel productsModel = new ProductModel
                {
                    Brand = product.Products.Brand,
                    Description = product.Products.Description,
                    Discount = product.Discount,
                    Name = product.Products.Name,
                    Price = product.Price,
                    TotalPrice = product.TotalPrice,
                    Type = product.Products.Type,
                    Category = product.Categories.Name,
                    Quantity = product.Quantity
                };

                productModels.Add(productsModel);
            }
            orderDetails.Products = productModels;
            return View(orderDetails);
        }
    }
}