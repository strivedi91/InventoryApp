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
    // Products
	
    internal partial class ProductsConfiguration : EntityTypeConfiguration<Products>
    {
		public ProductsConfiguration() : this("dbo")
		{
		}

        public ProductsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Products");
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName("id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasMaxLength(500);
            Property(x => x.Type).HasColumnName("Type").IsRequired().HasMaxLength(50);
            Property(x => x.Brand).HasColumnName("Brand").IsRequired().HasMaxLength(50);
            Property(x => x.Price).HasColumnName("Price").IsRequired().HasPrecision(18,2);
            Property(x => x.OfferPrice).HasColumnName("OfferPrice").IsOptional().HasPrecision(18,2);
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
            Property(x => x.MOQ).HasColumnName("MOQ").IsOptional();
            Property(x => x.CategoryId).HasColumnName("CategoryId").IsRequired();
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
            Property(x => x.IsDeleted).HasColumnName("IsDeleted").IsRequired();
            Property(x => x.MinimumSellingPrice).HasColumnName("MinimumSellingPrice").IsOptional().HasPrecision(18,2);
            Property(x => x.ApplyGst).HasColumnName("ApplyGst").IsRequired();

            // Foreign keys
            HasRequired(a => a.Categories).WithMany(b => b.Products).HasForeignKey(c => c.CategoryId); // FK_Products_Categories
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
