
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



    // AspNetUserDocumentTypes
	
    internal partial class AspNetUserDocumentTypesConfiguration : EntityTypeConfiguration<AspNetUserDocumentTypes>
    {
		public AspNetUserDocumentTypesConfiguration() : this("dbo")
		{
		}

        public AspNetUserDocumentTypesConfiguration(string schema = "dbo")
        {
 
           ToTable(schema + ".AspNetUserDocumentTypes");
 
           HasKey(x => x.id);


            Property(x => x.id).HasColumnName("id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);

            Property(x => x.DocumentId).HasColumnName("DocumentId").IsRequired();



            // Foreign keys

            HasRequired(a => a.AspNetUsers).WithMany(b => b.AspNetUserDocumentTypes).HasForeignKey(c => c.UserId); // FK_AspNetUserDocumentTypes_AspNetUsers

            HasRequired(a => a.DocumentTypes).WithMany(b => b.AspNetUserDocumentTypes).HasForeignKey(c => c.DocumentId); // FK_AspNetUserDocumentTypes_DocumentTypes



            InitializePartial();
        }

        partial void InitializePartial();
    }



}
