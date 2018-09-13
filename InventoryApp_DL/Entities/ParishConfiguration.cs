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
    // Parish
	
    internal partial class ParishConfiguration : EntityTypeConfiguration<Parish>
    {
		public ParishConfiguration() : this("dbo")
		{
		}

        public ParishConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Parish");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(50);
            Property(x => x.ParishName).HasColumnName("ParishName").IsRequired().HasMaxLength(100);
            Property(x => x.Address1).HasColumnName("Address1").IsRequired().HasMaxLength(100);
            Property(x => x.Address2).HasColumnName("Address2").IsOptional().HasMaxLength(100);
            Property(x => x.City).HasColumnName("City").IsRequired().HasMaxLength(50);
            Property(x => x.State).HasColumnName("State").IsRequired().HasMaxLength(50);
            Property(x => x.Zip).HasColumnName("Zip").IsRequired().HasMaxLength(50);
            Property(x => x.IsEnabled).HasColumnName("IsEnabled").IsOptional();
            Property(x => x.IsDeleted).HasColumnName("IsDeleted").IsRequired();
            Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired().HasMaxLength(128);
            Property(x => x.ModifiedOn).HasColumnName("ModifiedOn").IsOptional();
            Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").IsOptional().HasMaxLength(128);
            Property(x => x.RecordTimeStamp).HasColumnName("RecordTimeStamp").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
