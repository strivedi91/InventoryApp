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
    // Notifications
	
    internal partial class NotificationsConfiguration : EntityTypeConfiguration<Notifications>
    {
		public NotificationsConfiguration() : this("dbo")
		{
		}

        public NotificationsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Notifications");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.NotificationText).HasColumnName("NotificationText").IsRequired().HasMaxLength(500);
            Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
