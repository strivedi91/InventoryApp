
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



    // Offers
	
    internal partial class OffersConfiguration : EntityTypeConfiguration<Offers>
    {
		public OffersConfiguration() : this("dbo")
		{
		}

        public OffersConfiguration(string schema = "dbo")
        {
 
           ToTable(schema + ".Offers");
 
           HasKey(x => x.id);


            Property(x => x.id).HasColumnName("id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Offer).HasColumnName("Offer").IsRequired().HasMaxLength(10);

            Property(x => x.FlatDiscount).HasColumnName("FlatDiscount").IsRequired().HasPrecision(18,2);

            Property(x => x.PercentageDiscount).HasColumnName("PercentageDiscount").IsRequired();

            Property(x => x.ProductId).HasColumnName("ProductId").IsOptional();

            Property(x => x.CategoryId).HasColumnName("CategoryId").IsOptional();

            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();

            Property(x => x.IsDeleted).HasColumnName("IsDeleted").IsRequired();

            Property(x => x.StartDate).HasColumnName("StartDate").IsOptional();

            Property(x => x.EndDate).HasColumnName("EndDate").IsOptional();



            // Foreign keys

            HasOptional(a => a.Products).WithMany(b => b.Offers).HasForeignKey(c => c.ProductId); // FK_Offers_Products

            HasOptional(a => a.Categories).WithMany(b => b.Offers).HasForeignKey(c => c.CategoryId); // FK_Offers_Categories



            InitializePartial();
        }

        partial void InitializePartial();
    }



}
