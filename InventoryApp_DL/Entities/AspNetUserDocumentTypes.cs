
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
	[Serializable]

	
    public partial class AspNetUserDocumentTypes : InventoryApp_DL.Infrastructure.Entity
    {



        public int id { get; set; } // id (Primary key)


        public string UserId { get; set; } // UserId


        public int DocumentId { get; set; } // DocumentId




        // Foreign keys

        public virtual AspNetUsers AspNetUsers { get; set; } //  FK_AspNetUserDocumentTypes_AspNetUsers

        public virtual DocumentTypes DocumentTypes { get; set; } //  FK_AspNetUserDocumentTypes_DocumentTypes


    }



}
