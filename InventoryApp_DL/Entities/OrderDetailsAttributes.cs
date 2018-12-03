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
    // OrderDetailsAttributes
	[Serializable]
	
    public partial class OrderDetailsAttributes : InventoryApp_DL.Infrastructure.Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public int OrderDetailsId { get; set; } // OrderDetailsId
        public int ProductId { get; set; } // ProductId
        public string AttributeName { get; set; } // AttributeName
        public string AttributeValues { get; set; } // AttributeValues
        public string ControlType { get; set; } // ControlType

        // Foreign keys
        public virtual OrderDetails OrderDetails { get; set; } //  FK_OrderDetailsAttributes_OrderDetails
        public virtual Products Products { get; set; } //  FK_OrderDetailsAttributes_Products
    }

}
