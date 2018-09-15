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
    // AspNetUserLogins
	[Serializable]
	
    public partial class AspNetUserLogins : InventoryApp_DL.Infrastructure.Entity
    {
        public string LoginProvider { get; set; } // LoginProvider (Primary key)
        public string ProviderKey { get; set; } // ProviderKey (Primary key)
        public string UserId { get; set; } // UserId (Primary key)

        // Foreign keys
        public virtual AspNetUsers AspNetUsers { get; set; } //  FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId
    }

}
