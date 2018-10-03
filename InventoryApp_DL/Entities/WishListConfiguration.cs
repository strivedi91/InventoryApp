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
	
    internal partial class WishListConfiguration : EntityTypeConfiguration<WishList>
    {
		public WishListConfiguration() : this("dbo")
		{
		}

        public WishListConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".WishList");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CategoryId).HasColumnName("CategoryId").IsRequired();
            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();
            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);

            // Foreign keys
            HasRequired(a => a.Categories).WithMany(b => b.WishLists).HasForeignKey(c => c.CategoryId); // FK__WishList__Catego__19DFD96B
            HasRequired(a => a.Products).WithMany(b => b.WishLists).HasForeignKey(c => c.ProductId); // FK__WishList__Produc__1AD3FDA4
            HasRequired(a => a.AspNetUsers).WithMany(b => b.WishLists).HasForeignKey(c => c.UserId); // FK__WishList__UserId__1BC821DD
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
