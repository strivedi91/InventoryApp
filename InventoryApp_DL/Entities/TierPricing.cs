
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



    // TierPricing
	[Serializable]

	
    public partial class TierPricing : InventoryApp_DL.Infrastructure.Entity
    {



        public int Id { get; set; } // Id (Primary key)


        public int ProductId { get; set; } // ProductId


        public int QtyFrom { get; set; } // QtyFrom


        public int QtyTo { get; set; } // QtyTo


        public decimal Price { get; set; } // Price


        public bool IsActive { get; set; } // IsActive


        public bool IsDeleted { get; set; } // IsDeleted




        // Foreign keys

        public virtual Products Products { get; set; } //  FK_TierPricing_Products



        public TierPricing()
        {

            IsActive = true;

            IsDeleted = false;


            InitializePartial();
        }
        partial void InitializePartial();

    }





}
