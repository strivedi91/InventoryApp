
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



    // Orders
	[Serializable]

	
    public partial class Orders : InventoryApp_DL.Infrastructure.Entity
    {



        public int id { get; set; } // id (Primary key)


        public string UserId { get; set; } // UserId


        public DateTime CreatedOn { get; set; } // CreatedOn


        public decimal SubTotal { get; set; } // SubTotal


        public decimal Discount { get; set; } // Discount


        public decimal Total { get; set; } // Total


        public string OrderStatus { get; set; } // OrderStatus



        // Reverse navigation

        public virtual ICollection<OrderDetails> OrderDetails { get; set; } // OrderDetails.FK_OrderDetails_Order



        // Foreign keys

        public virtual AspNetUsers AspNetUsers { get; set; } //  FK_Orders_AspNetUsers



        public Orders()
        {

            CreatedOn = System.DateTime.Now;


            OrderDetails = new List<OrderDetails>();

            InitializePartial();
        }
        partial void InitializePartial();

    }



}
