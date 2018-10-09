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
    // ProductReview
	[Serializable]
	
    public partial class ProductReview : InventoryApp_DL.Infrastructure.Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public int ProductId { get; set; } // ProductId
        public decimal Rating { get; set; } // Rating
        public string Review { get; set; } // Review
        public string UserId { get; set; } // UserId
        public DateTime CreatedOn { get; set; } // CreatedOn

        // Foreign keys
        public virtual AspNetUsers AspNetUsers { get; set; } //  FK_ProductReview_AspNetUsers
        public virtual Products Products { get; set; } //  FK_ProductReview_Products

        public ProductReview()
        {
            CreatedOn = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
