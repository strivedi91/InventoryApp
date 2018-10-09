// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace InventoryApp_DL.Entities
{
    // OrderDetails
	[Serializable]
	
    public partial class OrderDetails : InventoryApp_DL.Infrastructure.Entity
    {
        public int id { get; set; } // id (Primary key)
        public int OrderId { get; set; } // OrderId
        public int CategoryId { get; set; } // CategoryId
        public int ProductId { get; set; } // ProductId
        public int Quantity { get; set; } // Quantity
        public decimal Price { get; set; } // Price
        public decimal Discount { get; set; } // Discount
        public decimal TotalPrice { get; set; } // TotalPrice
        public string OfferCode { get; set; } // OfferCode
        public string OfferDescription { get; set; } // OfferDescription
        public decimal? FlatDiscount { get; set; } // FlatDiscount
        public int? PercentageDiscount { get; set; } // PercentageDiscount

        // Foreign keys
        public virtual Categories Categories { get; set; } //  FK_OrderDetails_Categories
        public virtual Orders Orders { get; set; } //  FK_OrderDetails_Order
        public virtual Products Products { get; set; } //  FK_OrderDetails_Products
    }

}
