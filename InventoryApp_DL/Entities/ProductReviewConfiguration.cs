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
    // ProductReview
	
    internal partial class ProductReviewConfiguration : EntityTypeConfiguration<ProductReview>
    {
		public ProductReviewConfiguration() : this("dbo")
		{
		}

        public ProductReviewConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".ProductReview");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();
            Property(x => x.Rating).HasColumnName("Rating").IsRequired().HasPrecision(18,2);
            Property(x => x.Review).HasColumnName("Review").IsOptional().HasMaxLength(100);
            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);
            Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsRequired();

            // Foreign keys
            HasRequired(a => a.Products).WithMany(b => b.ProductReviews).HasForeignKey(c => c.ProductId); // FK_ProductReview_Products
            HasRequired(a => a.AspNetUsers).WithMany(b => b.ProductReviews).HasForeignKey(c => c.UserId); // FK_ProductReview_AspNetUsers
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
