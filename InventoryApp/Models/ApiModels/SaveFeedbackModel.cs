using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryApp.Models.ApiModels
{
    public class SaveProductReviewModel
    {
        public int ProductId { get; set; }
        public decimal Rating { get; set; }
        public string Review { get; set; }
    }
}