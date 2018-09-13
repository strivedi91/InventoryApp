
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



    // DocumentTypes
	[Serializable]

	
    public partial class DocumentTypes : InventoryApp_DL.Infrastructure.Entity
    {



        public int id { get; set; } // id (Primary key)


        public string DocumentType { get; set; } // DocumentType



        // Reverse navigation

        public virtual ICollection<AspNetUserDocumentTypes> AspNetUserDocumentTypes { get; set; } // AspNetUserDocumentTypes.FK_AspNetUserDocumentTypes_DocumentTypes




        public DocumentTypes()
        {


            AspNetUserDocumentTypes = new List<AspNetUserDocumentTypes>();

            InitializePartial();
        }
        partial void InitializePartial();

    }



}
