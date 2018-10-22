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
    // PageAccess
	
    internal partial class PageAccessConfiguration : EntityTypeConfiguration<PageAccess>
    {
		public PageAccessConfiguration() : this("dbo")
		{
		}

        public PageAccessConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".PageAccess");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PageId).HasColumnName("PageId").IsRequired();
            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);
            Property(x => x.IsAccessGranted).HasColumnName("IsAccessGranted").IsOptional();

            // Foreign keys
            HasRequired(a => a.Pages).WithMany(b => b.PageAccesses).HasForeignKey(c => c.PageId); // FK_PageAccess_Pages
            HasRequired(a => a.AspNetUsers).WithMany(b => b.PageAccesses).HasForeignKey(c => c.UserId); // FK_PageAccess_AspNetUsers
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
