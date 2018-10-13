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
	
    internal partial class CategoriesConfiguration : EntityTypeConfiguration<Categories>
    {
		public CategoriesConfiguration() : this("dbo")
		{
		}

        public CategoriesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Categories");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").IsRequired().HasMaxLength(500);
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
            Property(x => x.IsDeleted).HasColumnName("IsDeleted").IsRequired();
            Property(x => x.ParentId).HasColumnName("ParentId").IsRequired();
            Property(x => x.GstRate).HasColumnName("GstRate").IsOptional().HasPrecision(18,2);
            Property(x => x.GST).HasColumnName("GST").IsOptional().HasPrecision(18,2);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
