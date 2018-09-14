using InventoryApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryApp.Areas.Admin.Models
{
    public class UserModel
    {
        public UserModel()
        {
            loUserList = new List<UserViewModel>();
        }
        public Int64 inRecordCount { get; set; }
        public List<UserViewModel> loUserList { get; set; }
        public int inPageIndex { get; set; }
        public Pager Pager { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Name should not be empty.")]
        public string Name { get; set; }

        public string Address { get; set; }
        public decimal DepositAmount { get; set; }
        public int MembershipDuration { get; set; }
        public bool IsPaid { get; set; }
        public Nullable<System.DateTimeOffset> PaymentDate { get; set; }

        [Required(ErrorMessage = "Emil should not be empty.")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$",
        ErrorMessage = "Please Enter Correct Email Address")]
        public string Email { get; set; }

        [StringLength(10, ErrorMessage = "The Phone must contains 10 characters", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        public string stSearch { get; set; }
        public int inPageIndex { get; set; }
        public int inPageSize { get; set; }
        public string stSortColumn { get; set; }
        public string DocumentTypes { get; set; }
        public Int64 inRowNumber { get; set; }
        public List<UserDocumentTypeModel> loDocumentTypeList { get; set; }
    }

    public class UserDocumentTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public bool IsAdded { get; set; }
    }
}