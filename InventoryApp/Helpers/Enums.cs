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
        public enum OrderStatus : int
        {
            [Description("Order Placed")]
            OrderPlaced = 0,

            [Description("In Process")]
            InProcess = 1,

            [Description("Dispatched")]
            Dispatched = 2,

            [Description("Delivered/Completed")]
            DeliveredCompleted = 3
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