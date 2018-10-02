using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

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
    }
}