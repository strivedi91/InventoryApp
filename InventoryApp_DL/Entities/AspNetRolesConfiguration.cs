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
    // AspNetRoles
	
    internal partial class AspNetRolesConfiguration : EntityTypeConfiguration<AspNetRoles>
    {
		public AspNetRolesConfiguration() : this("dbo")
		{
		}

        public AspNetRolesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".AspNetRoles");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasMaxLength(128).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
            HasMany(t => t.AspNetUsers).WithMany(t => t.AspNetRoles).Map(m => 
            {
                m.ToTable("AspNetUserRoles");
                m.MapLeftKey("RoleId");
                m.MapRightKey("UserId");
            });
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
