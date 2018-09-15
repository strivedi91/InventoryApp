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
    // Cart
	
    internal partial class CartConfiguration : EntityTypeConfiguration<Cart>
    {
		public CartConfiguration() : this("dbo")
		{
		}

        public CartConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Cart");
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName("id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();
            Property(x => x.OfferId).HasColumnName("OfferId").IsOptional();
            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();

            // Foreign keys
            HasOptional(a => a.Products).WithMany(b => b.Carts).HasForeignKey(c => c.OfferId); // FK_Cart_Products
            HasRequired(a => a.AspNetUsers).WithMany(b => b.Carts).HasForeignKey(c => c.UserId); // FK_Cart_AspNetUsers
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
