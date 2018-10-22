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
    // NotificationHistory
	
    internal partial class NotificationHistoryConfiguration : EntityTypeConfiguration<NotificationHistory>
    {
		public NotificationHistoryConfiguration() : this("dbo")
		{
		}

        public NotificationHistoryConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".NotificationHistory");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NotificationText).HasColumnName("NotificationText").IsRequired().HasMaxLength(200);
            Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired().HasMaxLength(128);

            // Foreign keys
            HasRequired(a => a.AspNetUsers).WithMany(b => b.NotificationHistories).HasForeignKey(c => c.CreatedBy); // FK_NotificationHistory_AspNetUsers
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
