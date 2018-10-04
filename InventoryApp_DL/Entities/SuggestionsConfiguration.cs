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
    // Suggestions
	
    internal partial class SuggestionsConfiguration : EntityTypeConfiguration<Suggestions>
    {
		public SuggestionsConfiguration() : this("dbo")
		{
		}

        public SuggestionsConfiguration(string schema = "dbo")
        {
            ToTable(schema + ".Suggestions");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ProductId).HasColumnName("ProductId").IsRequired();
            Property(x => x.Suggestion).HasColumnName("Suggestion").IsOptional();

            // Foreign keys
            HasRequired(a => a.Products).WithMany(b => b.Suggestions).HasForeignKey(c => c.ProductId); // FK__Suggestio__Produ__2180FB33
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
