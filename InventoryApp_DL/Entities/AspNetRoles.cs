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
    // AspNetRoles
	[Serializable]
	
    public partial class AspNetRoles : InventoryApp_DL.Infrastructure.Entity
    {
        public string Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name

        // Reverse navigation
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; } // Many to many mapping

        public AspNetRoles()
        {
            AspNetUsers = new List<AspNetUsers>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
