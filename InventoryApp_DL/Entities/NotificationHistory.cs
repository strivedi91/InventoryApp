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
    // NotificationHistory
	[Serializable]
	
    public partial class NotificationHistory : InventoryApp_DL.Infrastructure.Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public string Title { get; set; } // Title
        public string NotificationText { get; set; } // NotificationText
        public DateTime CreatedOn { get; set; } // CreatedOn
        public string CreatedBy { get; set; } // CreatedBy

        // Foreign keys
        public virtual AspNetUsers AspNetUsers { get; set; } //  FK_NotificationHistory_AspNetUsers

        public NotificationHistory()
        {
            CreatedOn = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
