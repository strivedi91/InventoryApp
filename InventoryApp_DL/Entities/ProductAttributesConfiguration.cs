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
    // ProductAttributes
	
    internal partial class ProductAttributesConfiguration : EntityTypeConfiguration<ProductAttributes>
    {
		public ProductAttributesConfiguration() : this("dbo")
		{
		}

        public ProductAttributesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".ProductAttributes");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();
            Property(x => x.AttributeName).HasColumnName("AttributeName").IsRequired().HasMaxLength(50);
            Property(x => x.AttributeValues).HasColumnName("AttributeValues").IsRequired();
            Property(x => x.ControlType).HasColumnName("ControlType").IsRequired().HasMaxLength(50);

            // Foreign keys
            HasRequired(a => a.Products).WithMany(b => b.ProductAttributes).HasForeignKey(c => c.ProductId); // FK_ProductAttributes_Products
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
