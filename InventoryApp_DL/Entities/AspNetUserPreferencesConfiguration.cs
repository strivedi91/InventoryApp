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
    // AspNetUserPreferences
	
    internal partial class AspNetUserPreferencesConfiguration : EntityTypeConfiguration<AspNetUserPreferences>
    {
		public AspNetUserPreferencesConfiguration() : this("dbo")
		{
		}

        public AspNetUserPreferencesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".AspNetUserPreferences");
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName("id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);
            Property(x => x.CategoryId).HasColumnName("CategoryId").IsRequired();
            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();

            // Foreign keys
            HasRequired(a => a.AspNetUsers).WithMany(b => b.AspNetUserPreferences).HasForeignKey(c => c.UserId); // FK_AspNetUserCategories_AspNetUsers
            HasRequired(a => a.Categories).WithMany(b => b.AspNetUserPreferences).HasForeignKey(c => c.CategoryId); // FK_AspNetUserCategories_Categories
            HasRequired(a => a.Products).WithMany(b => b.AspNetUserPreferences).HasForeignKey(c => c.ProductId); // FK_AspNetUserPreferences_Products
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
