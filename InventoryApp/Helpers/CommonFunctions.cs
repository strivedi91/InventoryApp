using InventoryApp_DL.Entities;
using InventoryApp_DL.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace InventoryApp.Helpers
{
    public class CommonFunctions
    {
        public static string getCurrencyConverted(decimal amount)
        {
            string fare = amount.ToString();
            decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);
            CultureInfo hindi = new CultureInfo("hi-IN");
            string formatedAmount = string.Format(hindi, "{0:c}", parsed);
            return formatedAmount;
        }

        public static bool sendPlacedOrderEmail(Orders foOrder, List<OrderDetails> foOrderItems, string Status)
        {
            try
            {
                AspNetUsers loUser = Repository<AspNetUsers>.GetEntityListForQuery(x => x.Id == foOrder.UserId).Item1.FirstOrDefault();
                               
                string lsFrom = "no-replay@thgodowninventorryapp.com";

                string lsToMails = loUser.Email;

                string lsSubject = string.Empty;

                string lsEmailBody = string.Empty;

                lsSubject = "Your Order " + Status + " Successfully!";

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
                                    + "<td width='20%' style='border-bottom:1px solid #676161; padding:10px; text-align:right'>" + item.Price + "</td>"
                                    + "<td width='20%' style='border-bottom:1px solid #676161; padding:10px; text-align:right'>" + item.TotalPrice + "</td>"
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

        private static string[] GetProductImagesById(int productId)
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
                    ProductImages.Add(VirtualPathUtility.ToAbsolute(ProductImagePath) + productId.ToString() + "/" + fileName);
                }
                return ProductImages.ToArray();
            }
            
            else
            {
                return new string[] { };
            }
        }
    }
}