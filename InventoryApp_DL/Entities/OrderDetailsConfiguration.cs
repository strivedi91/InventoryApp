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
    // OrderDetails
	
    internal partial class OrderDetailsConfiguration : EntityTypeConfiguration<OrderDetails>
    {
		public OrderDetailsConfiguration() : this("dbo")
		{
		}

        public OrderDetailsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".OrderDetails");
            HasKey(x => x.id);

            Property(x => x.id).HasColumnName("id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.OrderId).HasColumnName("OrderId").IsRequired();
            Property(x => x.CategoryId).HasColumnName("CategoryId").IsRequired();
            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();
            Property(x => x.Quantity).HasColumnName("Quantity").IsRequired();
            Property(x => x.Price).HasColumnName("Price").IsRequired().HasPrecision(18,2);
            Property(x => x.Discount).HasColumnName("Discount").IsRequired().HasPrecision(18,2);
            Property(x => x.TotalPrice).HasColumnName("TotalPrice").IsRequired().HasPrecision(18,2);

            // Foreign keys
            HasRequired(a => a.Orders).WithMany(b => b.OrderDetails).HasForeignKey(c => c.OrderId); // FK_OrderDetails_Order
            HasRequired(a => a.Categories).WithMany(b => b.OrderDetails).HasForeignKey(c => c.CategoryId); // FK_OrderDetails_Categories
            HasRequired(a => a.Products).WithMany(b => b.OrderDetails).HasForeignKey(c => c.ProductId); // FK_OrderDetails_Products
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
