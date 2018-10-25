using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace InventoryApp.Helpers
{
    public class Enums
    {
        #region modules
        public enum Menus : int
        {
            [Description("Dashboard")]
            Dashboard = 0,

            [Description("Sellers")]
            Sellers = 1,

            [Description("Categories")]
            Categories = 2,

            [Description("Products")]
            Products = 3,

            [Description("Orders")]
            Orders = 4,

            [Description("Coupons")]
            Coupons = 5,

            [Description("Suggestions")]
            Suggestions = 6,

            [Description("Page Access")]
            Page_Access = 7
        }

        public enum OrderStatus : int
        {
            [Description("Order Placed")]
            OrderPlaced = 0,

            [Description("In Process")]
            InProcess = 1,

            [Description("Dispatched")]
            Dispatched = 2,

            [Description("Delivered")]
            DeliveredCompleted = 3,

            [Description("Cancelled")]
            Cancelled = 4,

            [Description("Requested Cancellation")]
            RequestedCancellation = 5
        }
        #endregion

        #region GetEnumDescription
        public static string GetEnumDescription(Enum value)
        {
            string description = string.Empty;
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                if (fi != null)
                {
                    DescriptionAttribute[] attributes =
                        (DescriptionAttribute[])fi.GetCustomAttributes(
                        typeof(DescriptionAttribute),
                        false);

                    if (attributes != null &&
                        attributes.Length > 0)
                        description = attributes[0].Description;
                    else
                        description = value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return description;
        }
        #endregion
    }


}