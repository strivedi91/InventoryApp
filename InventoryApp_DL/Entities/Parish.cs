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
    // Parish
	[Serializable]
	
    public partial class Parish : InventoryApp_DL.Infrastructure.Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public string Code { get; set; } // Code
        public string ParishName { get; set; } // ParishName
        public string Address1 { get; set; } // Address1
        public string Address2 { get; set; } // Address2
        public string City { get; set; } // City
        public string State { get; set; } // State
        public string Zip { get; set; } // Zip
        public bool? IsEnabled { get; set; } // IsEnabled
        public bool IsDeleted { get; set; } // IsDeleted
        public DateTime CreatedOn { get; set; } // CreatedOn
        public string CreatedBy { get; set; } // CreatedBy
        public DateTime? ModifiedOn { get; set; } // ModifiedOn
        public string ModifiedBy { get; set; } // ModifiedBy
        public byte[] RecordTimeStamp { get; internal set; } // RecordTimeStamp

        public Parish()
        {
            IsEnabled = false;
            IsDeleted = false;
            CreatedOn = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
