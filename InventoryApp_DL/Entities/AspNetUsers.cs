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
    // AspNetUsers
	[Serializable]
	
    public partial class AspNetUsers : InventoryApp_DL.Infrastructure.Entity
    {
        public string Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string Address { get; set; } // Address
        public decimal DepositAmount { get; set; } // DepositAmount
        public int MembershipDuration { get; set; } // MembershipDuration
        public bool IsPaid { get; set; } // IsPaid
        public DateTimeOffset? PaymentDate { get; set; } // PaymentDate
        public string Email { get; set; } // Email
        public bool EmailConfirmed { get; set; } // EmailConfirmed
        public string PasswordHash { get; set; } // PasswordHash
        public string SecurityStamp { get; set; } // SecurityStamp
        public string PhoneNumber { get; set; } // PhoneNumber
        public bool PhoneNumberConfirmed { get; set; } // PhoneNumberConfirmed
        public bool TwoFactorEnabled { get; set; } // TwoFactorEnabled
        public DateTime? LockoutEndDateUtc { get; set; } // LockoutEndDateUtc
        public bool LockoutEnabled { get; set; } // LockoutEnabled
        public int AccessFailedCount { get; set; } // AccessFailedCount
        public string UserName { get; set; } // UserName
        public bool IsActive { get; set; } // IsActive
        public bool IsDeleted { get; set; } // IsDeleted
        public string SecondaryPhone { get; set; } // SecondaryPhone

        // Reverse navigation
        public virtual ICollection<AspNetRoles> AspNetRoles { get; set; } // Many to many mapping
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; } // AspNetUserClaims.FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId
        public virtual ICollection<AspNetUserDocumentTypes> AspNetUserDocumentTypes { get; set; } // AspNetUserDocumentTypes.FK_AspNetUserDocumentTypes_AspNetUsers
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; } // Many to many mapping
        public virtual ICollection<AspNetUserPreferences> AspNetUserPreferences { get; set; } // AspNetUserPreferences.FK_AspNetUserCategories_AspNetUsers
        public virtual ICollection<Cart> Carts { get; set; } // Cart.FK_Cart_AspNetUsers
        public virtual ICollection<Orders> Orders { get; set; } // Orders.FK_Orders_AspNetUsers
        public virtual ICollection<ProductReview> ProductReviews { get; set; } // ProductReview.FK_ProductReview_AspNetUsers
        public virtual ICollection<Suggestions> Suggestions { get; set; } // Suggestions.FK__Suggestio__UserI__29221CFB
        public virtual ICollection<WishList> WishLists { get; set; } // WishList.FK__WishList__UserId__1BC821DD

        public AspNetUsers()
        {
            IsActive = true;
            IsDeleted = false;
            AspNetUserClaims = new List<AspNetUserClaims>();
            AspNetUserDocumentTypes = new List<AspNetUserDocumentTypes>();
            AspNetUserLogins = new List<AspNetUserLogins>();
            AspNetUserPreferences = new List<AspNetUserPreferences>();
            Carts = new List<Cart>();
            Orders = new List<Orders>();
            ProductReviews = new List<ProductReview>();
            Suggestions = new List<Suggestions>();
            WishLists = new List<WishList>();
            AspNetRoles = new List<AspNetRoles>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
