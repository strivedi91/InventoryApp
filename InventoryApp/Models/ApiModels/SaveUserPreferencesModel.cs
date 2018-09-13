using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryApp.Models.ApiModels
{
    public class SaveUserPreferencesModel
    {
        public int[] Categories { get; set; }
        public int[] Products { get; set; }
    }
}