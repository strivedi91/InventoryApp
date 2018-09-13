using InventoryApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal DepositAmount { get; set; }
        public int MembershipDuration { get; set; }
        public bool IsPaid { get; set; }
        public Nullable<System.DateTimeOffset> PaymentDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string stSearch { get; set; }
        public int inPageIndex { get; set; }
        public int inPageSize { get; set; }
        public string stSortColumn { get; set; }
        public Int64 inRowNumber { get; set; }
    }
}