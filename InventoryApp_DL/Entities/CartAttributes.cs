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
    // CartAttributes
	[Serializable]
	
    public partial class CartAttributes : InventoryApp_DL.Infrastructure.Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public int CartId { get; set; } // CartId
        public int ProductId { get; set; } // ProductId
        public string AttributeName { get; set; } // AttributeName
        public string AttributeValue { get; set; } // AttributeValue

        // Foreign keys
        public virtual Cart Cart { get; set; } //  FK_CartAttributes_Cart
        public virtual Products Products { get; set; } //  FK_CartAttributes_Products
    }

}
