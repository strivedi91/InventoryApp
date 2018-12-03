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
	
    internal partial class CartAttributesConfiguration : EntityTypeConfiguration<CartAttributes>
    {
		public CartAttributesConfiguration() : this("dbo")
		{
		}

        public CartAttributesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".CartAttributes");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CartId).HasColumnName("CartId").IsRequired();
            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();
            Property(x => x.AttributeName).HasColumnName("AttributeName").IsRequired().HasMaxLength(50);
            Property(x => x.AttributeValue).HasColumnName("AttributeValue").IsRequired();

            // Foreign keys
            HasRequired(a => a.Cart).WithMany(b => b.CartAttributes).HasForeignKey(c => c.CartId); // FK_CartAttributes_Cart
            HasRequired(a => a.Products).WithMany(b => b.CartAttributes).HasForeignKey(c => c.ProductId); // FK_CartAttributes_Products
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
