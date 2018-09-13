
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



    // TierPricing
	
    internal partial class TierPricingConfiguration : EntityTypeConfiguration<TierPricing>
    {
		public TierPricingConfiguration() : this("dbo")
		{
		}

        public TierPricingConfiguration(string schema = "dbo")
        {
 
           ToTable(schema + ".TierPricing");
 
           HasKey(x => x.Id);


            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();

            Property(x => x.QtyFrom).HasColumnName("QtyFrom").IsRequired();

            Property(x => x.QtyTo).HasColumnName("QtyTo").IsRequired();

            Property(x => x.Price).HasColumnName("Price").IsRequired().HasPrecision(18,2);

            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();

            Property(x => x.IsDeleted).HasColumnName("IsDeleted").IsRequired();



            // Foreign keys

            HasRequired(a => a.Products).WithMany(b => b.TierPricings).HasForeignKey(c => c.ProductId); // FK_TierPricing_Products



            InitializePartial();
        }

        partial void InitializePartial();
    }



}
