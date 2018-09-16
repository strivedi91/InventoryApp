using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryApp.Areas.Admin.Models
{
    public class OrderDetailsModel
    {
        public List<ProductModel> Products { get; set; }
    }

    public class ProductModel
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
    }

}