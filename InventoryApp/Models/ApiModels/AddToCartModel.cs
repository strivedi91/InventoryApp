using InventoryApp_DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryApp.Models.ApiModels
{
    public class AddToCartModel
    {
        public int ProductId { get; set; }
        public int OfferId { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public List<CartAttributes> cartAttributes { get; set; }

        public AddToCartModel()
        {
            cartAttributes = new List<CartAttributes>();
        }
    }

    public class UpdateCartModel
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public int OfferId { get; set; }
    }
}