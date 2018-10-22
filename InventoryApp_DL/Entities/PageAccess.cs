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
    // PageAccess
	[Serializable]
	
    public partial class PageAccess : InventoryApp_DL.Infrastructure.Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public int PageId { get; set; } // PageId
        public string UserId { get; set; } // UserId
        public bool? IsAccessGranted { get; set; } // IsAccessGranted

        // Foreign keys
        public virtual AspNetUsers AspNetUsers { get; set; } //  FK_PageAccess_AspNetUsers
        public virtual Pages Pages { get; set; } //  FK_PageAccess_Pages
    }

}
