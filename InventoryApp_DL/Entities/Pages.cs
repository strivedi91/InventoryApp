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
    // Pages
	[Serializable]
	
    public partial class Pages : InventoryApp_DL.Infrastructure.Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public string PageName { get; set; } // PageName
        public bool IsDeleted { get; set; } // IsDeleted

        // Reverse navigation
        public virtual ICollection<PageAccess> PageAccesses { get; set; } // PageAccess.FK_PageAccess_Pages

        public Pages()
        {
            PageAccesses = new List<PageAccess>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
