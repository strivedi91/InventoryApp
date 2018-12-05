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
    // OrderDetailsAttributes
	
    internal partial class OrderDetailsAttributesConfiguration : EntityTypeConfiguration<OrderDetailsAttributes>
    {
		public OrderDetailsAttributesConfiguration() : this("dbo")
		{
		}

        public OrderDetailsAttributesConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".OrderDetailsAttributes");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.OrderDetailsId).HasColumnName("OrderDetailsId").IsRequired();
            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();
            Property(x => x.AttributeName).HasColumnName("AttributeName").IsRequired().HasMaxLength(50);
            Property(x => x.AttributeValue).HasColumnName("AttributeValue").IsRequired();

            // Foreign keys
            HasRequired(a => a.OrderDetails).WithMany(b => b.OrderDetailsAttributes).HasForeignKey(c => c.OrderDetailsId); // FK_OrderDetailsAttributes_OrderDetails
            HasRequired(a => a.Products).WithMany(b => b.OrderDetailsAttributes).HasForeignKey(c => c.ProductId); // FK_OrderDetailsAttributes_Products
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
