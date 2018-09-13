
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



    // Categories
	[Serializable]

	
    public partial class Categories : InventoryApp_DL.Infrastructure.Entity
    {



        public int Id { get; set; } // Id (Primary key)


        public string Name { get; set; } // Name


        public string Description { get; set; } // Description


        public bool IsActive { get; set; } // IsActive


        public bool IsDeleted { get; set; } // IsDeleted


        public int ParentId { get; set; } // ParentId



        // Reverse navigation

        public virtual ICollection<AspNetUserPreferences> AspNetUserPreferences { get; set; } // AspNetUserPreferences.FK_AspNetUserCategories_Categories

        public virtual ICollection<Offers> Offers { get; set; } // Offers.FK_Offers_Categories

        public virtual ICollection<Products> Products { get; set; } // Products.FK_Products_Categories




        public Categories()
        {

            IsActive = true;

            IsDeleted = false;

            ParentId = 0;


            AspNetUserPreferences = new List<AspNetUserPreferences>();

            Offers = new List<Offers>();

            Products = new List<Products>();

            InitializePartial();
        }
        partial void InitializePartial();

    }



}
