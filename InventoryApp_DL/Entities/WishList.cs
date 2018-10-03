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
    // WishList
	[Serializable]
	
    public partial class WishList : InventoryApp_DL.Infrastructure.Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public int CategoryId { get; set; } // CategoryId
        public int ProductId { get; set; } // ProductId
        public string UserId { get; set; } // UserId

        // Foreign keys
        public virtual AspNetUsers AspNetUsers { get; set; } //  FK__WishList__UserId__1BC821DD
        public virtual Categories Categories { get; set; } //  FK__WishList__Catego__19DFD96B
        public virtual Products Products { get; set; } //  FK__WishList__Produc__1AD3FDA4
    }


}
