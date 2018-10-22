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
    // Pages
	
    internal partial class PagesConfiguration : EntityTypeConfiguration<Pages>
    {
		public PagesConfiguration() : this("dbo")
		{
		}

        public PagesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Pages");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.PageName).HasColumnName("PageName").IsRequired().HasMaxLength(100);
            Property(x => x.IsDeleted).HasColumnName("IsDeleted").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
