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
    // Orders
	
    internal partial class OrdersConfiguration : EntityTypeConfiguration<Orders>
    {
		public OrdersConfiguration() : this("dbo")
		{
		}

        public OrdersConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Orders");
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName("id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);
            Property(x => x.CreatedOn).HasColumnName("CreatedOn").IsRequired();
            Property(x => x.SubTotal).HasColumnName("SubTotal").IsRequired().HasPrecision(18,2);
            Property(x => x.Discount).HasColumnName("Discount").IsRequired().HasPrecision(18,2);
            Property(x => x.Total).HasColumnName("Total").IsRequired().HasPrecision(18,2);
            Property(x => x.OrderStatus).HasColumnName("OrderStatus").IsRequired().HasMaxLength(50);
            Property(x => x.ShippingAddress).HasColumnName("ShippingAddress").IsOptional().HasMaxLength(500);

            // Foreign keys
            HasRequired(a => a.AspNetUsers).WithMany(b => b.Orders).HasForeignKey(c => c.UserId); // FK_Orders_AspNetUsers
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
