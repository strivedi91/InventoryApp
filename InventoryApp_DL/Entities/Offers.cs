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
    // Offers
	[Serializable]
	
    public partial class Offers : InventoryApp_DL.Infrastructure.Entity
    {
        public int id { get; set; } // id (Primary key)
        public string OfferCode { get; set; } // OfferCode
        public string OfferDescription { get; set; } // OfferDescription
        public decimal FlatDiscount { get; set; } // FlatDiscount
        public int PercentageDiscount { get; set; } // PercentageDiscount
        public int? ProductId { get; set; } // ProductId
        public int? CategoryId { get; set; } // CategoryId
        public bool IsActive { get; set; } // IsActive
        public bool IsDeleted { get; set; } // IsDeleted
        public DateTime? StartDate { get; set; } // StartDate
        public DateTime? EndDate { get; set; } // EndDate

        // Reverse navigation
        public virtual ICollection<Cart> Carts { get; set; } // Cart.FK_Cart_Offers

        // Foreign keys
        public virtual Categories Categories { get; set; } //  FK_Offers_Categories
        public virtual Products Products { get; set; } //  FK_Offers_Products

        public Offers()
        {
            IsActive = true;
            IsDeleted = false;
            Carts = new List<Cart>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
