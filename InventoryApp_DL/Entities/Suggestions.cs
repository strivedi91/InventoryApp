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
    // Suggestions
	[Serializable]
	
    public partial class Suggestions : InventoryApp_DL.Infrastructure.Entity
    {
        public int Id { get; set; } // Id (Primary key)
        public int ProductId { get; set; } // ProductId
        public string UserId { get; set; } // UserId
        public string Suggestion { get; set; } // Suggestion
        public string SuggestionResponse { get; set; } // SuggestionResponse
        public DateTime? CreatedOn { get; set; } // CreatedOn
        public bool? IsReplied { get; set; } // IsReplied

        // Foreign keys
        public virtual AspNetUsers AspNetUsers { get; set; } //  FK__Suggestio__UserI__29221CFB
        public virtual Products Products { get; set; } //  FK__Suggestio__Produ__282DF8C2

        public Suggestions()
        {
            CreatedOn = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
