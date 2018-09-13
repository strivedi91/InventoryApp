
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



    // AspNetUserProducts
	
    internal partial class AspNetUserProductsConfiguration : EntityTypeConfiguration<AspNetUserProducts>
    {
		public AspNetUserProductsConfiguration() : this("dbo")
		{
		}

        public AspNetUserProductsConfiguration(string schema = "dbo")
        {
 
           ToTable(schema + ".AspNetUserProducts");
 
           HasKey(x => x.id);


            Property(x => x.id).HasColumnName("id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);

            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();



            // Foreign keys

            HasRequired(a => a.AspNetUsers).WithMany(b => b.AspNetUserProducts).HasForeignKey(c => c.UserId); // FK_AspNetUserProducts_AspNetUsers

            HasRequired(a => a.Products).WithMany(b => b.AspNetUserProducts).HasForeignKey(c => c.ProductId); // FK_AspNetUserProducts_Products



            InitializePartial();
        }

        partial void InitializePartial();
    }



}
