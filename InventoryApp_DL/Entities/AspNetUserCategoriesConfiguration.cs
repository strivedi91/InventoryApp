
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



    // AspNetUserCategories
	
    internal partial class AspNetUserCategoriesConfiguration : EntityTypeConfiguration<AspNetUserCategories>
    {
		public AspNetUserCategoriesConfiguration() : this("dbo")
		{
		}

        public AspNetUserCategoriesConfiguration(string schema = "dbo")
        {
 
           ToTable(schema + ".AspNetUserCategories");
 
           HasKey(x => x.id);


            Property(x => x.id).HasColumnName("id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);

            Property(x => x.CategoryId).HasColumnName("CategoryId").IsRequired();



            // Foreign keys

            HasRequired(a => a.AspNetUsers).WithMany(b => b.AspNetUserCategories).HasForeignKey(c => c.UserId); // FK_AspNetUserCategories_AspNetUsers

            HasRequired(a => a.Categories).WithMany(b => b.AspNetUserCategories).HasForeignKey(c => c.CategoryId); // FK_AspNetUserCategories_Categories



            InitializePartial();
        }

        partial void InitializePartial();
    }



}
